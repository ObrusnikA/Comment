using CommentingSystem.Data;
using CommentingSystem.Domain;
using CommentingSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System.Linq;
using X.PagedList;
using static System.Net.Mime.MediaTypeNames;

namespace CommentingSystem.Controllers;

[ValidateRecaptcha]
public class HomeController : Controller
{
	private readonly CommentingSystemDbContext _db;

	public HomeController(CommentingSystemDbContext db)
	{
		_db = db;
	}

	public async Task<IActionResult> Index(int? page = 1, string sortOrder = "")
	{
		if (page != null && page < 1)
		{
			page = 1;
		}

		var pageSize = 25;
		List<Comment> comments = await _db.Comments
			.AsNoTrackingWithIdentityResolution()
			.Include(c => c.Children)
			.ToListAsync();

		// Structure comments into a tree
		var rootCommentsQuery = comments
			.Where(c => c.ParentId == null)
			.AsParallel();

		ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
		ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
		ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
		switch (sortOrder)
		{
			case "email":
				rootCommentsQuery = rootCommentsQuery.OrderBy(s => s.Email);
				break;
			case "email_desc":
				rootCommentsQuery = rootCommentsQuery.OrderByDescending(s => s.Email);
				break;
			case "name":
				rootCommentsQuery = rootCommentsQuery.OrderBy(s => s.FullName);
				break;
			case "name_desc":
				rootCommentsQuery = rootCommentsQuery.OrderByDescending(s => s.FullName);
				break;
			case "date":
				rootCommentsQuery = rootCommentsQuery.OrderBy(s => s.DateCreated);
				break;
			case "date_desc":
				rootCommentsQuery = rootCommentsQuery.OrderByDescending(s => s.DateCreated);
				break;
			default:
				rootCommentsQuery = rootCommentsQuery.OrderByDescending(s => s.DateCreated);
				break;
		}

		var rootComments = rootCommentsQuery.ToPagedList(page ?? 1, pageSize);

		return View(rootComments);
	}

	[HttpPost]
	public async Task<IActionResult> CreateComment(CreateCommentDto commentCDto)
	{

		if (ModelState.IsValid)
		{
			var newComment = new Comment
			{
				ParentId = commentCDto.ReplyToCommentId,
				FullName = commentCDto.FullName,
				Email = commentCDto.Email,
				Message = commentCDto.Message,
				HomePage = commentCDto.HomePage,
				DateCreated = DateTimeOffset.Now,
				DateModified = DateTimeOffset.Now,
				IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
				WebBrowser = Request.Headers["User-Agent"].ToString()
			};
			using (var ms = new MemoryStream())
			{
				commentCDto.Image.CopyTo(ms);
				newComment.Image = ms.ToArray();
			}
			await _db.Comments.AddAsync(newComment);
			await _db.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// TODO: Throw some error
		return RedirectToAction(nameof(Index));
	}

	[HttpPost]
	public async Task<IActionResult> DeleteComment(int commentId)
	{
		var comments = await _db.Comments
			.Include(x => x.Children).ToListAsync();

		var flatten = Flatten(comments.Where(x => x.CommentId == commentId));

		_db.Comments.RemoveRange(flatten);

		await _db.SaveChangesAsync();

		return RedirectToAction(nameof(Index));
	}
	IEnumerable<Comment> Flatten(IEnumerable<Comment> comments) =>
		comments.SelectMany(x => Flatten(x.Children)).Concat(comments);
}
