using System;

namespace Infrastructure.DTOs.Generic
{
    public class PagedRequest
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;
        
        public int PageNumber 
        { 
            get => _pageNumber; 
            set => _pageNumber = value < 1 ? 1 : value; 
        }
        
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = value > 50 ? 50 : (value < 1 ? 10 : value); 
        }
        
        public string SortBy { get; set; } = "Id";
        public bool SortDesc { get; set; } = false;
        public string SearchTerm { get; set; } = string.Empty;
    }
} 