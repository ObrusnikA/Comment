namespace CommentingSystem.Domain;

public class Comment
{
	public int CommentId { get; set; }
	public int? ParentId { get; set; }
	public string FullName { get; set; }
	public string Email { get; set; }
	public string Message { get; set; }
	public byte[] Image { get; set; }
	public string HomePage { get; set; }
	public DateTimeOffset DateCreated { get; set; }
	public DateTimeOffset DateModified { get; set; }
	public Comment Parent { get; set; }
	public ICollection<Comment> Children { get; set; }
	public string IpAddress { get; init; }
	public string WebBrowser { get; init; }
}
