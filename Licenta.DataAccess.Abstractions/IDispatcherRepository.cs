﻿using System;
using System.Collections.Generic;
using System.Text;
using Licenta.Model;

namespace Licenta.DataAccess.Abstractions
{
    public interface IDispatcherRepository:IBaseRepository<Dispatcher>
    {
        Dispatcher GetByUserId(string userId);
    }
}
