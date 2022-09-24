using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CommentingSystem.DTOs;
public record class CreateCommentDto
{
	public int? ReplyToCommentId { get; init; }

	[Required, StringLength(1000, MinimumLength = 1), DisplayName("Message")]
	public string Message { get; init; }

	[EmailAddress, StringLength(100, MinimumLength = 6), Required]
	public string Email { get; init; }

	[Required, StringLength(60, MinimumLength = 3), DisplayName("Full Name")]
	public string FullName { get; init; }

	[Required]
	public IFormFile Image { get; init; }

	[Url]
	public string HomePage { get; init; }
}

