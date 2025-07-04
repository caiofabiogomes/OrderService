﻿namespace OrderService.Domain.Abstractions
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }

}
