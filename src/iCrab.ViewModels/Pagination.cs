﻿using System.Collections.Generic;

namespace iCrabee.ViewModels
{
    public class Pagination<T> : PaginationBase where T : class
    {
        public List<T> Items { get; set; }
    }
}
