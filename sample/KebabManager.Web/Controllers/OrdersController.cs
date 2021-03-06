using System.Collections.Generic;
using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Orders;
using Pizza.Mvc.Controllers;
using System.Web.Mvc;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.GridConfig;

namespace KebabManager.Web.Controllers
{
    [Authorize]
    public class OrdersController
        : CrudControllerBase<IOrdersService, OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>
    {
        public OrdersController(IOrdersService service)
            : base(service)
        {
        }

        protected override Dictionary<ViewType, string> ViewNames
        {
            get
            {
                return new Dictionary<ViewType, string> {
                    { ViewType.Index, "Orders list" },
                    { ViewType.Create, "New Order form" },
                    { ViewType.Edit, "Edit Order form" },
                    { ViewType.Details, "Order details" },
                };
            }
        }

        protected override GridMetamodel<OrderGridModel> GetGridMetamodel()
        {
            var gridMetaModel = new GridMetamodelBuilder<OrderGridModel>()
                .AllowNew("Create new Order").AllowEdit("Go to edit").AllowDelete("Delete!").AllowDetails("Go to details")
                .AddDataColumn(x => x.OrderDate, 200)
                .AddDataColumn(x => x.CustomerFirstName, 200)
                .AddDefaultSortColumn(x => x.CustomerLastName, SortMode.Descending, 150, ColumnWidthMode.Fixed, FilterOperator.Disabled)
                .AddDataColumn(x => x.ItemsCount, 150, ColumnWidthMode.Fixed, FilterOperator.DateEquals)
                .AddDataColumn(x => x.PaymentInfoNumber, 150, ColumnWidthMode.Fixed, FilterOperator.DateEquals)
                .Build();

            return gridMetaModel;
        }
    }
}