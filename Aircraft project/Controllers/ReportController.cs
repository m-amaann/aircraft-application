/*using Aircraft_project.Data;
using Aircraft_project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Aircraft_project.Controllers
{
    public class ReportController : Controller
    {
        private readonly IPdfGenerator _pdfGenerator;
        private readonly ApplicationDbContext _context;

        public ReportController(IPdfGenerator pdfGenerator, ApplicationDbContext context)
        {
            _pdfGenerator = pdfGenerator;
            _context = context;
        }


        public IActionResult GenerateAdminReport()
        {
            var htmlContent = GetHtmlContent();
            var pdfContent = _pdfGenerator.GeneratePdf(htmlContent);
            return File(pdfContent, "application/pdf", "AdminReport.pdf");
        }


        private string GetHtmlContent()
        {
            var admins = _context.Admins.ToList();

            var html= new StringBuilder();
            html.Append("<html><body>");
            html.Append("<h1>Admin Report</h1>");
            html.Append("<table border='1'><tr><th>Name</th><th>Email</th><th>Created At</th></tr>");

            foreach (var admin in admins)
            {
                html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
                                admin.Name,
                                admin.Email,
                                admin.CreatedAt.ToString("yyyy-MM-dd"));
            }

            html.Append("</table>");
            html.Append("</body></html>");

            return html.ToString();
        }
    }
}
*/