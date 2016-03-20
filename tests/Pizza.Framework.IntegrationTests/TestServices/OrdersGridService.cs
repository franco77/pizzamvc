﻿using System;
using NHibernate;
using Pizza.Framework.Operations;
using Pizza.Framework.Persistence.Transactions;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Orders;
using Pizza.Framework.Utils.ValueInjection;

namespace Pizza.Framework.IntegrationTests.TestServices
{
    [Transactional]
    public class OrdersCrudService :
        CrudServiceBase<Order, OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>,
        IOrdersCrudService
    {
        public OrdersCrudService(ISession session) : base(session) { }

        public override int Create(OrderCreateModel createModel)
        {
            var customer = this.session.Get<Customer>(createModel.CustomerId);

            var order = new Order();
            order.InjectFromViewModel(createModel);
            order.Customer = customer;
            order.PaymentInfo.OrderedDate = DateTime.Now;

            return (int)this.session.Save(order);
        }
    }
}
