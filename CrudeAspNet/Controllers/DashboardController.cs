using System.Net.Http.Json;
using CrudeAspNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudeAspNet.Controllers;

public class DashboardController(IHttpClientFactory httpClientFactory) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = new KierDashboardViewModel();
        var api = httpClientFactory.CreateClient("KierCrudApi");

        try
        {
            var health = await api.GetAsync("api/health", cancellationToken);
            health.EnsureSuccessStatusCode();
            model.Students = await api.GetFromJsonAsync<List<KierStudent>>("api/students", cancellationToken) ?? [];
            model.Courses = await api.GetFromJsonAsync<List<KierCourse>>("api/courses", cancellationToken) ?? [];
            model.SchoolYears = await api.GetFromJsonAsync<List<KierSchoolYear>>("api/schoolyears", cancellationToken) ?? [];
            model.Enrollments = await api.GetFromJsonAsync<List<KierEnrollment>>("api/enrollments", cancellationToken) ?? [];
            model.IsAvailable = true;
        }
        catch (HttpRequestException)
        {
            model.ErrorMessage = "KierCRUD API is not available. Start it on http://localhost:5100 and refresh this page.";
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            model.ErrorMessage = "KierCRUD API took too long to respond. Check that it is running and refresh this page.";
        }

        return View(model);
    }
}