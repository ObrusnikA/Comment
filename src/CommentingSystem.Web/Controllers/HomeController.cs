using CommentingSystem.Data;
using CommentingSystem.Domain;
using CommentingSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
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

	public async Task<IActionResult> Index(int? page = 1)
	{
		if (page != null && page < 1)
		{
			page = 1;
		}
		var pageSize = 2;
		List<Comment> comments = await _db.Comments
			.AsNoTrackingWithIdentityResolution()
			.Include(c => c.Children)
			.OrderByDescending(x => x.DateCreated)
			.ToListAsync();

		// Structure comments into a tree
		var rootComments = comments
			.Where(c => c.ParentId == null)
			.AsParallel()
			.OrderByDescending(x => x.DateCreated)
			.ToPagedList(page ?? 1, pageSize);

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
				DateModified = DateTimeOffset.Now
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
