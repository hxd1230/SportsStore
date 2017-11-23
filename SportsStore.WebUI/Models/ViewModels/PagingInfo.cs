using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.WebUI.Models.ViewModels
{
    public class PagingInfo
    {
        /// <summary>
        /// 总数RecordCount
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// 每页显示数PageSize
        /// </summary>
        public int ItermsPerPage { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / ItermsPerPage);
            }
        }
    }
}