using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Globe.Identity.AdministrativeDashboard.Client.Services
{
    public class TableSortService
    {
        public Action<IEnumerable<object>> Sorted { get; set; }

        bool _isSortedAscending;
        string _activeSortColumn;

        [Parameter]
        public string IconSortUp { get; set; } = "fa-sort-up";
        public string IconSortDown { get; set; } = "fa-sort-down";

        public IEnumerable<object> Sort(string columnName, IEnumerable<object> items)
        {
            if (columnName != _activeSortColumn)
            {
                items = items.OrderBy(item => item.GetType().GetProperty(columnName).GetValue(item, null)).ToList();
                _isSortedAscending = true;
                _activeSortColumn = columnName;
            }
            else
            {
                if (_isSortedAscending)
                {
                    items = items.OrderByDescending(item => item.GetType().GetProperty(columnName).GetValue(item, null)).ToList();
                }
                else
                {
                    items = items.OrderBy(item => item.GetType().GetProperty(columnName).GetValue(item, null)).ToList();
                }
                _isSortedAscending = !_isSortedAscending;
            }

            Sorted?.Invoke(items);

            return items;
        }

        public string SortIcon(string columnName)
        {
            if (_activeSortColumn != columnName)
            {
                return string.Empty;
            }
            if (_isSortedAscending)
            {
                return IconSortUp;
            }
            else
            {
                return IconSortDown;
            }
        }
    }
}
