﻿using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 分页列表接口
/// </summary>
public interface IPagedList : IEnumerable
{
    int CurrentPageIndex { get; set; }
    int PageSize { get; set; }
    int TotalItemCount { get; set; }
}

public interface IPagedList<T> : IEnumerable<T>, IPagedList { }