namespace FileUploader.Repositories.Contracts;

public interface IViewRenderService
{
    Task<string> RenderToStringAsync(string viewName, object model);
}