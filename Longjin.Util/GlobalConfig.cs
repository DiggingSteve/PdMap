using System.Configuration;

namespace Longjin.Util
{
    public class GlobalConfig
    {
        /// <summary>
        /// 附件请求的地址
        /// </summary>
        public static string AttachmentRequestURL = string.Format("{0}Att.aspx", ConfigurationManager.AppSettings["ApiSiteUrl"]);
        
        /// <summary>
        /// Api站点请求地址
        /// </summary>
        public static string ApiSiteUrl = ConfigurationManager.AppSettings["ApiSiteUrl"];

        /// <summary>
        /// 样板图的枚举值
        /// </summary>
        public static string ENUM_Att_SourceType_Templet = ConfigurationManager.AppSettings["Attachment_SourceType_Templet"];

        /// <summary>
        /// 头像的枚举值
        /// </summary>
        public static string ENUM_Att_SourceType_Avatar = ConfigurationManager.AppSettings["Attachment_SourceType_Avatar"];

        /// <summary>
        /// 会议室状态枚举值
        /// </summary>
        public static string ENUM_MEETINGROOM_STATUS = ConfigurationManager.AppSettings["MEETINGROOM_STATUS"];

        /// <summary>
        /// 会议室可用枚举值
        /// </summary> 
        public static string ENUM_MEETINGROOM_STATUS_USABLE = ConfigurationManager.AppSettings["MEETINGROOM_STATUS_USABLE"];

        /// <summary>
        /// 场地租用枚举值
        /// </summary>
        public static string ENUM_RENT_TYPE = ConfigurationManager.AppSettings["RENT_TYPE"];

        /// <summary>
        /// 企业标签父枚举值
        /// </summary>
        public static string ENUM_ENTERPRISER_TAG = ConfigurationManager.AppSettings["ENTERPRISER_TAG"];

        /// <summary>
        /// 活动枚举值
        /// </summary>
        public static string ENUM_Att_SourceType_Activity = ConfigurationManager.AppSettings["Attachment_SourceType_Activity"];

        /// <summary>
        /// 一寸照的枚举值
        /// </summary>
        public static string ENUM_Att_SourceType_Photo = ConfigurationManager.AppSettings["Attachment_SourceType_Photo"];

        /// <summary>
        /// 文章Logo
        /// </summary>
        public static string ENUM_Att_SourceType_Airtle = ConfigurationManager.AppSettings["Attachment_SourceType_Airtle"];

        /// <summary>
        /// 简历的枚举值
        /// </summary>
        public static string ENUM_Att_SourceType_Vita = ConfigurationManager.AppSettings["Attachment_SourceType_Vita"];

        ///<summary>
        ///性别的枚举值
        ///</summary>
        public static string ENUM_GENDER = ConfigurationManager.AppSettings["GENDER"];

        ///<summary>
        ///学历的枚举值
        ///</summary>
        public static string ENUM_EDUCATION = ConfigurationManager.AppSettings["EDUCATION"];

        ///<summary>
        ///政治面貌的枚举值
        ///</summary>
        public static string ENUM_POLITICAL = ConfigurationManager.AppSettings["POLITICAL"];

        ///<summary>
        ///职位类别的枚举值
        ///</summary>
        public static string ENUM_POSITION = ConfigurationManager.AppSettings["POSITION"];

        ///<summary>
        ///个人简历状态的枚举值
        ///</summary>
        public static string ENUM_PERSONALVITASTATUS = ConfigurationManager.AppSettings["PERSONALVITASTATUS"];

        ///<summary>
        ///个人简历审核中的枚举值
        ///</summary>
        public static string ENUM_PERSONALVITASTATUS_PENDING = ConfigurationManager.AppSettings["PERSONALVITASTATUS_PENDING"];

        ///<summary>
        ///个人简历审核通过的枚举值
        ///</summary>
        public static string ENUM_PERSONALVITASTATUS_PASSED = ConfigurationManager.AppSettings["PERSONALVITASTATUS_PASSED"];

        ///<summary>
        ///个人简历审核不通过的枚举值
        ///</summary>
        public static string ENUM_PERSONALVITASTATUS_NOTPASSED = ConfigurationManager.AppSettings["PERSONALVITASTATUS_NOTPASSED"];

        ///<summary>
        ///行业类别的枚举值
        ///</summary>
        public static string ENUM_TRADE = ConfigurationManager.AppSettings["TRADE"];

        ///<summary>
        ///签证类别的枚举值
        ///</summary>
        public static string ENUM_VISATYPE = ConfigurationManager.AppSettings["VISATYPE"];

        ///<summary>
        ///签证状态的枚举值
        ///</summary>
        public static string ENUM_VISASTATUS = ConfigurationManager.AppSettings["VISASTATUS"];

        ///<summary>
        ///签证状态审核中的枚举值
        ///</summary>
        public static string ENUM_VISASTATUS_PENDING = ConfigurationManager.AppSettings["VISASTATUS_PENDING"];

        ///<summary>
        ///签证状态审核通过的枚举值
        ///</summary>
        public static string ENUM_VISASTATUS_PASSED = ConfigurationManager.AppSettings["VISASTATUS_PASSED"];

        ///<summary>
        ///签证状态审核不通过的枚举值
        ///</summary>
        public static string ENUM_VISASTATUS_NOTPASSED = ConfigurationManager.AppSettings["VISASTATUS_NOTPASSED"];

        ///<summary>
        ///个人简历求职状态
        ///</summary>
        public static string ENUM_PERSONALJOBSTATUS = ConfigurationManager.AppSettings["PERSONALJOBSTATUS"];

        /// <summary>
        /// 到岗时间
        /// </summary>
        public static string ENUM_DUTYTIME = ConfigurationManager.AppSettings["DUTYTIME"];

        ///<summary>
        ///企业需求状态的枚举值
        ///</summary>
        public static string ENUM_ENTERPRISESTATUS = ConfigurationManager.AppSettings["ENTERPRISESTATUS"];

        ///<summary>
        ///企业需求状态审核中的枚举值
        ///</summary>
        public static string ENUM_ENTERPRISE_PENDING = ConfigurationManager.AppSettings["ENTERPRISE_PENDING"];

        ///<summary>
        ///企业需求状态审核通过的枚举值
        ///</summary>
        public static string ENUM_ENTERPRISE_PASSED = ConfigurationManager.AppSettings["ENTERPRISE_PASSED"];

        ///<summary>
        ///企业需求状态审核不通过的枚举值
        ///</summary>
        public static string ENUM_ENTERPRISE_NOTPASSED = ConfigurationManager.AppSettings["ENTERPRISE_NOTPASSED"];

        /// <summary>
        /// 人员类别
        /// </summary>
        public static string ENUM_PERSONAL_CLASS = ConfigurationManager.AppSettings["PERSONAL_CLASS"];

        /// <summary>
        /// 居住事由
        /// </summary>
        public static string ENUM_PERSONAL_LIVINGMASTER = ConfigurationManager.AppSettings["PERSONAL_LIVINGMASTER"];

        /// <summary>
        /// 文章归属平台
        /// </summary>
        public static string ENUM_ARTICLEPLATFORM = ConfigurationManager.AppSettings["ARTICLEPLATFORM"];

        /// <summary>
        /// 订阅平台类型
        /// </summary>
        public static string ENUM_SUBSCRIPTIONTYPE = ConfigurationManager.AppSettings["SUBSCRIPTIONTYPE"];

        /// <summary>
        /// 订阅平台类型-劳动法
        /// </summary>
        public static string ENUM_SUBSCRIPTION_WORK = ConfigurationManager.AppSettings["SUBSCRIPTION_WORK"];
        /// <summary>
        /// 订阅平台类型-自贸区政策
        /// </summary>
        public static string ENUM_SUBSCRIPTION_FTA = ConfigurationManager.AppSettings["SUBSCRIPTION_FTA"];
        /// <summary>
        /// 订阅平台类型-四金
        /// </summary>
        public static string ENUM_SUBSCRIPTION_GOLD = ConfigurationManager.AppSettings["SUBSCRIPTION_GOLD"];
        /// <summary>
        /// 订阅平台类型-自人力资源政策
        /// </summary>
        public static string ENUM_SUBSCRIPTION_PERSONAL = ConfigurationManager.AppSettings["SUBSCRIPTION_PERSONAL"];
        /// <summary>
        /// 订阅平台类型-其他
        /// </summary>
        public static string ENUM_SUBSCRIPTION_OTHER = ConfigurationManager.AppSettings["SUBSCRIPTION_OTHER"];

        /// <summary>
        /// 文章归属平台-一卡通优惠信息
        /// </summary>
        public static string ENUM_ARTICLEPLATFORM_YKTYHXX = ConfigurationManager.AppSettings["ARTICLEPLATFORM_YKTYHXX"];

        /// <summary>
        /// 文章归属平台-人才政策
        /// </summary>
        public static string ENUM_ARTICLEPLATFORM_RCZC = ConfigurationManager.AppSettings["ARTICLEPLATFORM_RCZC"];

        /// <summary>
        /// 文章归属平台-职场资讯
        /// </summary>
        public static string ENUM_ARTICLEPLATFORM_ZCZX = ConfigurationManager.AppSettings["ARTICLEPLATFORM_ZCZX"];

        /// <summary>
        /// 文章归属平台-实时动态
        /// </summary>
        public static string ENUM_ARTICLEPLATFORM_SSDT = ConfigurationManager.AppSettings["ARTICLEPLATFORM_SSDT"];

        /// <summary>
        /// 文章归属平台-生活小贴士
        /// </summary>
        public static string ENUM_ARTICLEPLATFORM_SSXTS = ConfigurationManager.AppSettings["ARTICLEPLATFORM_SHXTS"];

        /// <summary>
        /// 文章-金致生活-教育
        /// </summary>
        public static string ENUM_ARTICLEPLATFORM_EDUCATION = ConfigurationManager.AppSettings["ARTICLEPLATFORM_EDUCATION"];

        /// <summary>
        /// 平台-园区管理后台
        /// </summary>
        public static string ENUM_PLATFORM_ADMIN = ConfigurationManager.AppSettings["PLATFORM_ADMIN"];

        /// <summary>
        /// 平台-个人APP
        /// </summary>
        public static string ENUM_PLATFORM_PERSONALAPP = ConfigurationManager.AppSettings["PLATFORM_PERSONALAPP"];
        /// <summary>
        /// 平台-个人
        /// </summary>
        public static string ENUM_PLATFORM_PERSONAL = ConfigurationManager.AppSettings["PLATFORM_PERSONAL"];

        /// <summary>
        /// 平台-个人PC
        /// </summary>
        public static string ENUM_PLATFORM_PERSONALPC = ConfigurationManager.AppSettings["PLATFORM_PERSONALPC"];

        /// <summary>
        /// 平台-企业
        /// </summary>
        public static string ENUM_PLATFORM_ENTERPRISE = ConfigurationManager.AppSettings["PLATFORM_ENTERPRISE"];

        /// <summary>
        /// 一卡通状态-正常
        /// </summary>
        public static string ENUM_CARDSTATUS_NORMAL = ConfigurationManager.AppSettings["CARDSTATUS_NORMAL"];

        /// <summary>
        /// 一卡通状态-挂失
        /// </summary>
        public static string ENUM_CARDSTATUS_LOSS = ConfigurationManager.AppSettings["CARDSTATUS_LOSS"];

        /// <summary>
        /// 一卡通状态-注销
        /// </summary>
        public static string ENUM_CARDSTATUS_CANCEL = ConfigurationManager.AppSettings["CARDSTATUS_CANCEL"];

        /// <summary>
        /// 一卡通操作
        /// </summary>
        public static string ENUM_OPERATETYPE = ConfigurationManager.AppSettings["OPERATETYPE"];

        /// <summary>
        /// 一卡通操作-绑定
        /// </summary>
        public static string ENUM_OPERATETYPE_BIND = ConfigurationManager.AppSettings["OPERATETYPE_BIND"];
        /// <summary>
        /// 一卡通操作-挂失
        /// </summary>
        public static string ENUM_OPERATETYPE_LOSS = ConfigurationManager.AppSettings["OPERATETYPE_LOSS"];
        /// <summary>
        /// 一卡通操作-获得积分
        /// </summary>
        public static string ENUM_OPERATETYPE_GET = ConfigurationManager.AppSettings["OPERATETYPE_GET"];
        /// <summary>
        /// 一卡通操作-积分消费
        /// </summary>
        public static string ENUM_OPERATETYPE_AREA = ConfigurationManager.AppSettings["OPERATETYPE_AREA"];

        /// <summary>
        /// 积分类型
        /// </summary>
        public static string ENUM_INTEGRALTYPE = ConfigurationManager.AppSettings["INTEGRALTYPE"];
        /// <summary>
        /// 环保类型
        /// </summary>
        public static string ENUM_CREDITTYPE_ENVIRONMENTAL = ConfigurationManager.AppSettings["ENVIR_TYPE"];
        /// <summary>
        /// 餐饮积分
        /// </summary>
        public static string ENUM_CREDITTYPE_CATERING = ConfigurationManager.AppSettings["RESTAURANT_TYPE"];
        /// <summary>
        /// 文化积分
        /// </summary>
        public static string ENUM_CREDITTYPE_CULTURE = ConfigurationManager.AppSettings["CUULTRUE_TYPE"];
        /// <summary>
        /// 我的积分
        /// </summary>
        public static string ENUM_CREDITTYPE_INTEGRAL = ConfigurationManager.AppSettings["INTEGRAL_TYPE"];

        /// <summary>
        /// 房源状态
        /// </summary>
        public static string ENUM_APARTMENTSTATUS = ConfigurationManager.AppSettings["APARTMENTSTATUS"];

        /// <summary>
        /// 户型
        /// </summary>
        public static string ENUM_LAYOUT = ConfigurationManager.AppSettings["LAYOUT"];

        /// <summary>
        /// 生活-天气请求URL地址
        /// </summary>
        public static string WeatherRequestURL = ConfigurationManager.AppSettings["WeatherRequestURL"];

        /// <summary>
        /// 生活-常用快递请求URL地址
        /// </summary>
        public static string ExpressRequestURL = ConfigurationManager.AppSettings["ExpressRequestURL"];

        /// <summary>
        /// 生活-公交请求URL地址
        /// </summary>
        public static string BusRequestURL = ConfigurationManager.AppSettings["BusRequestURL"];

        /// <summary>
        /// 生活-空气质量
        /// </summary>
        public static string AirQualityRequestURL = ConfigurationManager.AppSettings["AirQualityRequestURL"];

        /// <summary>
        /// 生活-手机号码归属地请求URL地址
        /// </summary>
        public static string MobileAttRequestURL = ConfigurationManager.AppSettings["MobileAttRequestURL"];

        /// <summary>
        /// 生活-新闻头条请求URL地址
        /// </summary>
        public static string HeadlinesRequestURL = ConfigurationManager.AppSettings["HeadlinesRequestURL"];

        /// <summary>
        /// 生活-万年历请求URL地址
        /// </summary>
        public static string PerpetualCalendarRequestURL = ConfigurationManager.AppSettings["PerpetualCalendarRequestURL"];

        /// <summary>
        /// 生活-邮编信息请求URL地址
        /// </summary>
        public static string ZipDetailsRequestURL = ConfigurationManager.AppSettings["ZipDetailsRequestURL"];

        /// <summary>
        /// 生活-微信精选请求URL地址
        /// </summary>
        public static string WeChatDetailsRequestURL = ConfigurationManager.AppSettings["WeChatDetailsRequestURL"];

        /// <summary>
        /// 商铺请求URL地址
        /// </summary>
        public static string ShopRequestURL = ConfigurationManager.AppSettings["ShopRequestURL"];

        /// <summary>
        /// 自行车-登录认证接口
        /// </summary>
        public static string BicycleLoginRequestURL = ConfigurationManager.AppSettings["BicycleLoginRequestURL"];

        /// <summary>
        /// 自行车-会员信息获取接口
        /// </summary>
        public static string BicycleMemberInfoRequestURL = ConfigurationManager.AppSettings["BicycleMemberInfoRequestURL"];

        /// <summary>
        /// 自行车-会员卡信息获取接口
        /// </summary>
        public static string BicycleCardInfoRequestURL = ConfigurationManager.AppSettings["BicycleCardInfoRequestURL"];

        /// <summary>
        /// 自行车-交易记录获取接口
        /// </summary>
        public static string BicycleConsumeInfoRequestURL = ConfigurationManager.AppSettings["BicycleConsumeInfoRequestURL"];

        /// <summary>
        /// 自行车-运营区域获取接口
        /// </summary>
        public static string BicycleRunlevelRequestURL = ConfigurationManager.AppSettings["BicycleRunlevelRequestURL"];

        /// <summary>
        /// 自行车-网点列表获取接口
        /// </summary>
        public static string BicycleBaseInfoBatchRequestURL = ConfigurationManager.AppSettings["BicycleBaseInfoBatchRequestURL"];

        /// <summary>
        /// 自行车-单个网点信息获取接口
        /// </summary>
        public static string BicycleBaseInfoRequestURL = ConfigurationManager.AppSettings["BicycleBaseInfoRequestURL"];

        /// <summary>
        /// 自行车-网点锁柱列表获取接口
        /// </summary>
        public static string BicycleDeviceInfoRequestURL = ConfigurationManager.AppSettings["BicycleDeviceInfoRequestURL"];

        /// <summary>
        /// 自行车-借车接口
        /// </summary>
        public static string BicycleLendRequestURL = ConfigurationManager.AppSettings["BicycleLendRequestURL"];

        /// <summary>
        /// 自行车-基础配置获取接口
        /// </summary>
        public static string BicycleBaseConfigRequestURL = ConfigurationManager.AppSettings["BicycleBaseConfigRequestURL"];

        /// <summary>
        /// 自行车-所有网点GPS坐标获取接口
        /// </summary>
        public static string BicycleGpsRequestURL = ConfigurationManager.AppSettings["BicycleGpsRequestURL"];

        /// <summary>
        /// 自行车-获取某个坐标的最近网点
        /// </summary>
        public static string BicycleGetShortestRequestURL = ConfigurationManager.AppSettings["BicycleGetShortestRequestURL"];

        /// <summary>
        /// 人才公寓审批状态类型
        /// </summary>
        public static string ApartmentApplicationType = ConfigurationManager.AppSettings["APARTMENTAPPLICATION"];

        /// <summary>
        /// HR审核中
        /// </summary>
        public static string ApartmentHRPending = ConfigurationManager.AppSettings["APARTMENTAPPLICATION_HRPENDING"];
        /// <summary>
        /// HR拒绝
        /// </summary>
        public static string ApartmentHRReject = ConfigurationManager.AppSettings["APARTMENTAPPLICATION_HRREJECT"];

        /// <summary>
        /// 园区审核中
        /// </summary>
        public static string ApartmentZonePending = ConfigurationManager.AppSettings["APARTMENTAPPLICATION_ZONEPENDING"];

        /// <summary>
        ///园区拒绝 
        /// </summary>
        public static string ApartmentZoneReject = ConfigurationManager.AppSettings["APARTMENTAPPLICATION_ZONEREJECT"];

        /// <summary>
        /// 通过（排队中）
        /// </summary>
        public static string ApartmentPass = ConfigurationManager.AppSettings["APARTMENTAPPLICATION_PASS"];

        /// <summary>
        /// 已入住
        /// </summary>
        public static string ApartmentCheckIn = ConfigurationManager.AppSettings["APARTMENTAPPLICATION_CHECKIN"];

        /// <summary>
        /// 主动放弃
        /// </summary>
        public static string ApartmentAbandon = ConfigurationManager.AppSettings["APARTMENTAPPLICATION_ABANDON"];
        /// <summary>
        /// 证件类型
        /// </summary>
        public static string IDTYPE = ConfigurationManager.AppSettings["IDTYPE"];
        /// <summary>
        /// 身份证
        /// </summary>
        public static string IDTYPE_ID = ConfigurationManager.AppSettings["IDTYPE_ID"];

        /// <summary>
        /// 护照
        /// </summary>
        public static string IDTYPE_PASSPORT = ConfigurationManager.AppSettings["IDTYPE_PASSPORT"];

        /// <summary>
        /// 港澳通行证
        /// </summary>
        public static string IDTYPE_HONGKONG = ConfigurationManager.AppSettings["IDTYPE_HONGKONG"];

        /// <summary>
        /// 台湾通行证
        /// </summary>
        public static string IDTYPE_TAIWAN = ConfigurationManager.AppSettings["IDTYPE_TAIWAN"];

        /// <summary>
        /// 其他
        /// </summary>
        public static string IDTYPE_OTHERS = ConfigurationManager.AppSettings["IDTYPE_OTHERS"];

        /// <summary>
        /// 车辆情况
        /// </summary>
        public static string VEHICLE = ConfigurationManager.AppSettings["VEHICLE"];

        /// <summary>
        /// 私家车
        /// </summary>
        public static string VEHICLE_CAR = ConfigurationManager.AppSettings["VEHICLE_CAR"];

        /// <summary>
        /// 电动车
        /// </summary>
        public static string VEHICLE_ELECTRIC = ConfigurationManager.AppSettings["VEHICLE_ELECTRIC"];

        /// <summary>
        /// 自行车
        /// </summary>
        public static string VEHICLE_MOBIKE = ConfigurationManager.AppSettings["VEHICLE_MOBIKE"];

        /// <summary>
        /// 其他
        /// </summary>
        public static string VEHICLE_OTHERS = ConfigurationManager.AppSettings["VEHICLE_OTHERS"];

        /// <summary>
        /// 希望使用年限
        /// </summary>
        public static string LEASE = ConfigurationManager.AppSettings["LEASE"];

        /// <summary>
        /// 一年
        /// </summary>
        public static string LEASE_ONE = ConfigurationManager.AppSettings["LEASE_ONE"];

        /// <summary>
        /// 两年
        /// </summary>
        public static string LEASE_TWO = ConfigurationManager.AppSettings["LEASE_TWO"];

        /// <summary>
        /// 三年
        /// </summary>
        public static string LEASE_THREE = ConfigurationManager.AppSettings["LEASE_THREE"];

        /// <summary>
        /// 四年
        /// </summary>
        public static string LEASE_FOUR = ConfigurationManager.AppSettings["LEASE_FOUR"];

        /// <summary>
        /// 五年
        /// </summary>
        public static string LEASE_FIVE = ConfigurationManager.AppSettings["LEASE_FIVE"];

        /// <summary>
        /// 签约主体
        /// </summary>
        public static string CONTRACTSUBJECT = ConfigurationManager.AppSettings["CONTRACTSUBJECT"];

        /// <summary>
        /// 签约主体-企业
        /// </summary>
        public static string CONTRACTSUBJECT_ENTERPRISE = ConfigurationManager.AppSettings["CONTRACTSUBJECT_ENTERPRISE"];

        /// <summary>
        /// 签约主体-个人
        /// </summary>
        public static string CONTRACTSUBJECT_PERSONAL = ConfigurationManager.AppSettings["CONTRACTSUBJECT_PERSONAL"];

        /// <summary>
        /// 收入
        /// </summary>
        public static string INCOME = ConfigurationManager.AppSettings["INCOME"];

        /// <summary>
        /// 5001-10000
        /// </summary>
        public static string INCOME_LOW = ConfigurationManager.AppSettings["INCOME_LOW"];

        /// <summary>
        /// 10001-15000
        /// </summary>
        public static string INCOME_MEDIUM = ConfigurationManager.AppSettings["INCOME_MEDIUM"];

        /// <summary>
        /// 15001-20000
        /// </summary>
        public static string INCOME_HIGH = ConfigurationManager.AppSettings["INCOME_HIGH"];
        /// <summary>
        /// 20000以上
        /// </summary>
        public static string INCOME_VERYHIGH = ConfigurationManager.AppSettings["INCOME_VERYHIGH"];

        /// <summary>
        /// 企业账号职位
        /// </summary>
        public static string ENTERPRISE_USER_POSITION = ConfigurationManager.AppSettings["ENTERPRISEUSERPOSITION"];

        /// <summary>
        /// 企业账号职位-HR
        /// </summary>
        public static string ENTERPRISE_USER_POSITION_HRMANAGER = ConfigurationManager.AppSettings["ENTERPRISEUSERPOSITION_HRMANAGER"];

        /// <summary>
        /// 总经理
        /// </summary>
        public static string ENTERPRISE_USER_GENERALMANAGER = ConfigurationManager.AppSettings["ENTERPRISEUSERPOSITION_GENERALMANAGER"];

        /// <summary>
        /// 网上审批大厅地址父类
        /// </summary>
        public static string APPROVALTYPE = ConfigurationManager.AppSettings["APPROVALTYPE"];

        /// <summary>
        /// 入驻申请状态不通过枚举值
        /// </summary>
        public static string ENUM_ENTERAPPLY_STATUS_NOTPASS = ConfigurationManager.AppSettings["ENTERAPPLY_STATUS_NOTPASS"];

        /// <summary>
        /// 入驻申请状态通过枚举值
        /// </summary>
        public static string ENUM_ENTERAPPLY_STATUS_PASS = ConfigurationManager.AppSettings["ENTERAPPLY_STATUS_PASS"];

        /// <summary>
        /// 入驻申请状态审核中枚举值
        /// </summary>
        public static string ENUM_ENTERAPPLY_STATUS_PENDING = ConfigurationManager.AppSettings["ENTERAPPLY_STATUS_PENDING"];

        /// <summary>
        /// 企业申请账号状态
        /// </summary>
        public static string ENTERPRISE_APPLY_ACCOUNT_STATUS = ConfigurationManager.AppSettings["ENTERPRISE_APPLY_ACCOUNT_STATUS"];

        /// <summary>
        /// 审核中
        /// </summary>
        public static string ENTERPRISE_APPLY_ACCOUNT_PENDING = ConfigurationManager.AppSettings["ENTERPRISE_APPLY_ACCOUNT_PENDING"];

        /// <summary>
        /// 通过
        /// </summary>
        public static string ENTERPRISE_APPLY_ACCOUNT_APPROVAL = ConfigurationManager.AppSettings["ENTERPRISE_APPLY_ACCOUNT_APPROVAL"];

        /// <summary>
        /// 拒绝
        /// </summary>
        public static string ENTERPRISE_APPLY_ACCOUNT_REJECT = ConfigurationManager.AppSettings["ENTERPRISE_APPLY_ACCOUNT_REJECT"];

        /// <summary>
        /// 快捷审批-赴美签证
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_FMQZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_FMQZ"];

        /// <summary>
        /// 快捷审批-澳大利亚旅游签证
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_ADLYLYQZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_ADLYLYQZ"];

        /// <summary>
        /// 快捷审批-加拿大旅游签证
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_JNDLYQZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_JNDLYQZ"];

        /// <summary>
        /// 快捷审批-日本签证
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_RBQZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_RBQZ"];

        /// <summary>
        /// 快捷审批-韩国签证
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_HGQZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_HGQZ"];

        /// <summary>
        /// 快捷审批-英国签证
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_YGQZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_YGQZ"];

        /// <summary>
        /// 快捷审批-居住证积分
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_JZZJF = ConfigurationManager.AppSettings["QUICKAPPROVAL_JZZJF"];

        /// <summary>
        /// 快捷审批-出境快速通道
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_CRJKSTD = ConfigurationManager.AppSettings["QUICKAPPROVAL_CRJKSTD"];

        /// <summary>
        /// 快捷审批--R签证
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_RQZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_RQZ"];

        /// <summary>
        /// 快捷审批--外国专家
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_WGZJ = ConfigurationManager.AppSettings["QUICKAPPROVAL_WGZJ"];

        /// <summary>
        /// 快捷审批--外国人就业证
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_WGRJYZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_WGRJYZ"];

        /// <summary>
        /// 快捷审批--工作居留许可
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_GZJLXKZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_GZJLXKZ"];

        /// <summary>
        /// 快捷审批--B类居住证
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_BLJZZZ = ConfigurationManager.AppSettings["QUICKAPPROVAL_BLJZZ"];

        /// <summary>
        /// 快捷审批--入境快速通道
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_RJKSTD = ConfigurationManager.AppSettings["QUICKAPPROVAL_RJKSTD"];

        /// <summary>
        ///快捷审批状态- 审核中
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_STATUS_CHECK = ConfigurationManager.AppSettings["QUICKAPPROVAL_STATUS_CHECK"];

        /// <summary>
        ///快捷审批状态- 通过
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_STATUS_PASS = ConfigurationManager.AppSettings["QUICKAPPROVAL_STATUS_PASS"];

        /// <summary>
        /// 快捷审批状态-拒绝
        /// </summary>
        public static string ENUM_QUICKAPPROVAL_STATUS_FAIL = ConfigurationManager.AppSettings["QUICKAPPROVAL_STATUS_FAIL"];

        /// <summary>
        /// 企业生命周期管理状态
        /// </summary>
        public static string POTENTIAL_ENTERPRISE_STATUS = ConfigurationManager.AppSettings["POTENTIAL_ENTERPRISE_STATUS"];

        /// <summary>
        /// 潜在企业，联系中
        /// </summary>
        public static string POTENTIAL_ENTERPRISE_STATUS_CONTACTING = ConfigurationManager.AppSettings["POTENTIAL_ENTERPRISE_STATUS_CONTACTING"];

        /// <summary>
        /// 潜在企业，已入驻
        /// </summary>
        public static string POTENTIAL_ENTERPRISE_STATUS_SETTLED = ConfigurationManager.AppSettings["POTENTIAL_ENTERPRISE_STATUS_SETTLED"];
        /// <summary>
        /// 潜在企业，已退出
        /// </summary>
        public static string POTENTIAL_ENTERPRISE_STATUS_SIGNOUT = ConfigurationManager.AppSettings["POTENTIAL_ENTERPRISE_STATUS_SIGNOUT"];

        /// <summary>
        /// 企业生命周期企业日志类型
        /// </summary>
        public static string ENTERPRISELIFE_LOG_TYPE = ConfigurationManager.AppSettings["ENTERPRISELIFE_LOG_TYPE"];

        /// <summary>
        /// 企业日志类型普通
        /// </summary>
        public static string ENTERPRISELIFE_LOG_TYPE_NORMAL = ConfigurationManager.AppSettings["ENTERPRISELIFE_LOG_TYPE_NORMAL"];

        /// <summary>
        /// 企业日志类型退出
        /// </summary>
        public static string ENTERPRISELIFE_LOG_TYPE_SIGNOUT = ConfigurationManager.AppSettings["ENTERPRISELIFE_LOG_TYPE_SIGNOUT"];

        /// <summary>
        /// App模块
        /// </summary>
        public static string ENUM_APP_MODULE = ConfigurationManager.AppSettings["APP_MODULE"];

        /// <summary>
        /// App模块-首页
        /// </summary>
        public static string ENUM_APP_MODULE_HOME = ConfigurationManager.AppSettings["APP_MODULE_HOME"];

        /// <summary>
        /// App模块-人才直通车
        /// </summary>
        public static string ENUM_APP_MODULE_TALENT = ConfigurationManager.AppSettings["APP_MODULE_TALENT"];

        /// <summary>
        /// App模块-金致生活
        /// </summary>
        public static string ENUM_APP_MODULE_LIFE = ConfigurationManager.AppSettings["APP_MODULE_LIFE"];

        /// <summary>
        /// App模块-一卡通
        /// </summary>
        public static string ENUM_APP_MODULE_CARD = ConfigurationManager.AppSettings["APP_MODULE_CARD"];

        /// <summary>
        /// 活动类型-人才培训
        /// </summary>
        public static string ENUM_ACTIVITY_TYPE_TRAIN = ConfigurationManager.AppSettings["ACTIVITY_TYPE_TRAIN"];

        /// <summary>
        /// 一卡通卡操作类型-简历填写（个人）
        /// </summary>
        public static string ENUM_CARD_HANDLE_TYPE_CAREER = ConfigurationManager.AppSettings["CARD_HANDLE_TYPE_CAREER"];

        /// <summary>
        /// 一卡通卡操作类型-实名认证（企业）
        /// </summary>
        public static string ENUM_CARD_HANDLE_TYPE_CERTIFICATION = ConfigurationManager.AppSettings["CARD_HANDLE_TYPE_CERTIFICATION"];

        /// <summary>
        /// 一卡通卡操作类型-快捷审批
        /// </summary>
        public static string ENUM_CARD_HANDLE_TYPE_QUICK_APPROVAL = ConfigurationManager.AppSettings["CARD_HANDLE_TYPE_QUICK_APPROVAL"];

        /// <summary>
        /// 一卡通卡操作类型-人才培训
        /// </summary>
        public static string ENUM_CARD_HANDLE_TYPE_TRAINING = ConfigurationManager.AppSettings["CARD_HANDLE_TYPE_TRAINING"];

        /// <summary>
        /// 短息inbox类型
        /// </summary>
        public static string ENUM_SMS_TEMPLATE= ConfigurationManager.AppSettings["SMS_TEMPLATE"];

        /// <summary>
        /// 短信模板
        /// </summary>
        public static string SMS_TEMPLATE_SMS = ConfigurationManager.AppSettings["SMS_TEMPLATE_SMS"];

        /// <summary>
        /// inbox模板
        /// </summary>
        public static string SMS_TEMPLATE_INBOX = ConfigurationManager.AppSettings["SMS_TEMPLATE_INBOX"];

        /// <summary>
        /// 服务-生活小贴士
        /// </summary>
        public static string SERVICE_SHXTS = ConfigurationManager.AppSettings["SERVICE_SHXTS"];
        /// <summary>
        ///  服务-天气
        /// </summary>
        public static string SERVICE_WEATHER = ConfigurationManager.AppSettings["SERVICE_WEATHER"];
        /// <summary>
        ///  服务-教育
        /// </summary>
        public static string SERVICE_EDUCATION = ConfigurationManager.AppSettings["SERVICE_EDUCATION"];

    }
}
