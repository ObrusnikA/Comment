namespace CommentingSystem.Web.Configurations
{
	public class Pager
	{
		public int TotalItems { get; init; }
		public int CurrentPage { get; init; }
		public int PageSize { get; init; }
		
		public int TotalPages { get; init; }
		public int StartPage { get; init; }
		public int EndPage { get; init; }

		public Pager()
		{

		}

		public Pager(int totalItems, int page, int pageSize = 10)
		{
			int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
			int currentPage = page;

			int startPage = currentPage - 5;
			int endPage = currentPage + 4;

			if (startPage <= 0)
			{
				endPage = endPage - (startPage - 1);
				startPage = 1;
			}

			if (endPage > totalPages)
			{
				endPage = totalPages;
				if (endPage > 10)
				{
					startPage = endPage - 9;
				}
			}

			TotalItems = totalItems;
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalPages = totalPages;
			StartPage = startPage;
			EndPage = endPage;
		}
	}
}
