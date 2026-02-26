using PayerPortalAutomation.Application.DTOs;
using PayerPortalAutomation.Application.Interfaces;
using PayerPortalAutomation.Application.Repositories;
using PayerPortalAutomation.Automation.Core;
using PayerPortalAutomation.Domain.Entities;
using PayerPortalAutomation.Infrastructure.Repositories;
using PayerPortalAutomation.Infrastructure.Storage;

namespace PayerPortalAutomation.Automation.Portals;

public class DemoPortalAutomation :
    PortalAutomationBase,
    IPortalAutomation
    {

    public async Task<EobRunResult> RetrieveEobsAsync(
        PayerTarget payer,
        EobRequest request,
        string runId)
    {
        var storage = new FileArtifactStorage();
        var runFolder = storage.CreateRunFolder(runId);
        var eobFolder = storage.GetEobFolder(runId);

        var page = await CreatePageAsync();

        await LoginAsync(page, payer.PortalUrl, payer.Username, payer.Password);

        await page.ClickAsync("a[href='/eobs']");
        await page.FillAsync("#tax-id-input", request.TaxId);
        await page.FillAsync("#date-from-input", request.DateFrom.ToString("yyyy-MM-dd"));
        await page.FillAsync("#date-to-input", request.DateTo.ToString("yyyy-MM-dd"));
        await page.ClickAsync("button[type=submit]");

        await page.WaitForSelectorAsync(".data-table tbody tr");

        var eobRows = page.Locator(".data-table tbody tr");

        var results = new List<EobRecord>();

        int downloaded = 0;
        int skippedByDedupe = 0;

        var processedInRun = new HashSet<string>();

        var totalFound = await eobRows.CountAsync();

        var limit = Math.Min(totalFound, request.MaxEobs);

        for (int i = 0; i < limit; i++)
        {
            var row = eobRows.Nth(i);

            var eobId = (await row.Locator("td").Nth(0).InnerTextAsync()).Trim();

            var serviceDateText =
                await row.Locator("td").Nth(1).InnerTextAsync();

            var serviceDate = DateTime.Parse(serviceDateText);

            var pdfPath = Path.Combine(eobFolder, $"{eobId}.pdf");


            //------------------------------------
            // DEDUPE בתוך אותו RUN
            //------------------------------------

            if (request.Dedupe?.Mode == "eobId"
                && request.Dedupe.Scope == "run"
                && processedInRun.Contains(eobId))
            {
                skippedByDedupe++;
                continue;
            }

            processedInRun.Add(eobId);


            //------------------------------------
            // DOWNLOAD
            //------------------------------------

            if (request.Download)
            {
                bool alreadyExists = File.Exists(pdfPath);

                bool shouldDownload =
                    request.ForceRedownload
                    || !alreadyExists;

                if (!shouldDownload)
                {
                    skippedByDedupe++;
                }
                else
                {
                    var button = row.Locator("td:last-child button");

                    var download = await page.RunAndWaitForDownloadAsync(async () =>
                    {
                        await button.ClickAsync();
                    });

                    await download.SaveAsAsync(pdfPath);

                    downloaded++;


                    //------------------------------------
                    // RATE LIMIT
                    //------------------------------------

                    if (payer.DefaultMinDelayMs > 0)
                        await Task.Delay(payer.DefaultMinDelayMs);
                }
            }


            //------------------------------------
            // ADD RESULT
            //------------------------------------

            results.Add(new EobRecord
            {
                EobId = eobId,
                ServiceDate = serviceDate,
                PdfPath = pdfPath
            });
        }



        //------------------------------------
        // RETURN RUN SUMMARY
        //------------------------------------

        return new EobRunResult
        {
            RunId = runId,
            Status = "completed",

            PayerId = payer.PayerId,

            TaxId = request.TaxId,
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,

            EobsFound = totalFound,

            Downloaded = downloaded,

            SkippedByDedupe = skippedByDedupe,

            Items = results
        };
    
    }

    public async Task<AppealRunResult> SubmitAppealAsync(
        PayerTarget payer,
        AppealRequest request,
        string runId)
    {
        var storage = new FileArtifactStorage();
        var runFolder = storage.CreateRunFolder(runId);

        var page = await CreatePageAsync();

        await LoginAsync(page, payer.PortalUrl, payer.Username, payer.Password);

        await page.ClickAsync("a[href='/appeals/new']");

        await page.FillAsync("#appeal-tax-id", request.TaxId);

        await page.FillAsync("#appeal-claim-id", request.ClaimId);
        await page.SelectOptionAsync("#appeal-type-select", new[] { request.AppealType });

        await page.FillAsync("#appeal-reason-text", request.Reason);

        foreach (var attachment in request.Attachments ?? Array.Empty<AttachmentDto>())
        {
            var filePath = Path.Combine(runFolder, attachment.FileName);
            await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(attachment.ContentBase64));
            await page.SetInputFilesAsync("#appeal-file-input", filePath);
        }

        if (request.Submit)
        {
            await page.ClickAsync("button[type=submit]");

            await page.WaitForSelectorAsync(".confirmation-id");

            if (payer.DefaultMinDelayMs > 0)
                await Task.Delay(payer.DefaultMinDelayMs);
        }

        var confirmationId = await page
            .Locator(".confirmation-id span")
            .Nth(2)
            .InnerTextAsync();

        return new AppealRunResult
        {
            RunId = runId,
            Status = "Completed",
            PayerId = payer.PayerId,
            TaxId = request.TaxId,
            ClaimId = request.ClaimId,
            ConfirmationId = confirmationId
        };
    }
}