using System;
using System.Collections.Generic;

namespace Infrastructure.DTOs.Generic
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Data { get; set; } = new List<T>();
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public PagedResponse(List<T> data, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Data = data;
        }

        public static PagedResponse<T> Create(List<T> data, int count, int pageNumber, int pageSize)
        {
            return new PagedResponse<T>(data, count, pageNumber, pageSize);
        }
    }
} 