using ProductionApp.UserInterface.Models;
using ProductionApp.DataAccessObject.DTO;
using SAMTool.DataAccessObject.DTO;

namespace ProductionApp.UserInterface.App_Start
{
    public class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                //domain <===== viewmodel
                //viewmodel =====> domain
                //ReverseMap() makes it possible to map both ways.


                //*****SAMTOOL MODELS 
                config.CreateMap<LoginViewModel, SAMTool.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<UserViewModel, SAMTool.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<SysMenuViewModel, SAMTool.DataAccessObject.DTO.SysMenu>().ReverseMap();
                config.CreateMap<RolesViewModel, SAMTool.DataAccessObject.DTO.Roles>().ReverseMap();
                config.CreateMap<ApplicationViewModel, SAMTool.DataAccessObject.DTO.Application>().ReverseMap();
                config.CreateMap<AppObjectViewModel, SAMTool.DataAccessObject.DTO.AppObject>().ReverseMap();
                config.CreateMap<AppSubobjectViewmodel, SAMTool.DataAccessObject.DTO.AppSubobject>().ReverseMap();
                config.CreateMap<CommonViewModel, SAMTool.DataAccessObject.DTO.Common>().ReverseMap();
                config.CreateMap<ManageAccessViewModel, SAMTool.DataAccessObject.DTO.ManageAccess>().ReverseMap();
                config.CreateMap<ManageSubObjectAccessViewModel, SAMTool.DataAccessObject.DTO.ManageSubObjectAccess>().ReverseMap();
                config.CreateMap<PrivilegesViewModel, SAMTool.DataAccessObject.DTO.Privileges>().ReverseMap();


                //****SAMTOOL MODELS END

                //****PRODUCTION APP MODELS
                config.CreateMap<AMCSysModuleViewModel, AMCSysModule>().ReverseMap();
                config.CreateMap<AMCSysMenuViewModel, AMCSysMenu>().ReverseMap();
                config.CreateMap<DataTablePagingViewModel, DataTablePaging>().ReverseMap();
                config.CreateMap<CommonViewModel, DataAccessObject.DTO.Common>().ReverseMap();

                config.CreateMap<BankViewModel, Bank>().ReverseMap();
                config.CreateMap<UserViewModel, ProductionApp.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<BankAdvanceSearchViewModel, BankAdvanceSearch>().ReverseMap();
                config.CreateMap<MaterialIssueViewModel, MaterialIssue>().ReverseMap();
                config.CreateMap<MaterialIssueDetailViewModel, MaterialIssueDetail>().ReverseMap();
                config.CreateMap<MaterialViewModel, Material>().ReverseMap();
                config.CreateMap<MaterialAdvanceSearchViewModel, MaterialAdvanceSearch>().ReverseMap();
                config.CreateMap<UnitViewModel, Unit>().ReverseMap();
                config.CreateMap<DocumentTypeViewModel, DocumentType>().ReverseMap();
                config.CreateMap<MaterialTypeViewModel, MaterialType>().ReverseMap();
                config.CreateMap<ProductViewModel, Product>().ReverseMap();
                config.CreateMap<ProductAdvanceSearchViewModel, ProductAdvanceSearch>().ReverseMap();
                config.CreateMap<ApproverViewModel, Approver>().ReverseMap();
                config.CreateMap<ApproverAdvanceSearchViewModel, ApproverAdvanceSearch>().ReverseMap();
                config.CreateMap<PurchaseOrderViewModel, PurchaseOrder>().ReverseMap();
                config.CreateMap<PurchaseOrderAdvanceSearchViewModel, PurchaseOrderAdvanceSearch>().ReverseMap();
                config.CreateMap<SupplierViewModel, Supplier>().ReverseMap();

                config.CreateMap<RequisitionViewModel, Requisition>().ReverseMap();
                config.CreateMap<RequisitionDetailViewModel, RequisitionDetail>().ReverseMap();
                config.CreateMap<RequisitionAdvanceSearchViewModel, RequisitionAdvanceSearch>().ReverseMap();
                config.CreateMap<EmployeeViewModel, Employee>().ReverseMap();
                config.CreateMap<MaterialStockAdjViewModel, MaterialStockAdj>().ReverseMap();
                config.CreateMap<MaterialStockAdjAdvanceSearchViewModel, MaterialStockAdjAdvanceSearch>().ReverseMap();
                config.CreateMap<MaterialReceiptViewModel, MaterialReceipt>().ReverseMap();
                config.CreateMap<MaterialReceiptAdvanceSearchViewModel, MaterialReceiptAdvanceSearch>().ReverseMap();
                config.CreateMap<MaterialIssueAdvanceSearchViewModel, MaterialIssueAdvanceSearch>().ReverseMap();
                config.CreateMap<DocumentApprovalViewModel, DocumentApproval>().ReverseMap();
                config.CreateMap<DocumentApprovalAdvanceSearchViewModel, DocumentApprovalAdvanceSearch>().ReverseMap();
                config.CreateMap<SalesSummaryViewModel, SalesSummary>().ReverseMap();
                config.CreateMap<PurchaseSummaryViewModel, PurchaseSummary>().ReverseMap();
                config.CreateMap<IncomeExpenseSummaryViewModel, IncomeExpenseSummary>().ReverseMap();
                config.CreateMap<DocumentApproverViewModel, DocumentApprover>().ReverseMap();
            });
        }
    }
}