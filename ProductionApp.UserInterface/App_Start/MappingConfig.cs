﻿using ProductionApp.UserInterface.Models;
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
                config.CreateMap<BankAdvanceSearchViewModel, BankAdvanceSearch>().ReverseMap();

                config.CreateMap<PurchaseOrderHeaderViewModel, PurchaseOrderHeader>().ReverseMap();
                config.CreateMap<PurchaseOrderAdvanceSearchViewModel, PurchaseOrderAdvanceSearch>().ReverseMap();

            });
        }
    }
}