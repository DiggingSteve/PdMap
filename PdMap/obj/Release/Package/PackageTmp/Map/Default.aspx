<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PdMap.Web.Map.Defalut" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Modules/map/maptalks.css" rel="stylesheet" />
    
    <link href="../js/layer/skin/layer.css" rel="stylesheet" />
    <link href="../css/main.css?4" rel="stylesheet" />

</head>
<body>
    <div class="main_wrap">
        <div class="menu_wrap"></div>
        <div class="legend_btn_wrap">
            <input id="btn_legend" value="图例" type="button" />
        </div>
        <div class="searh_wrap">
            <select class="select" id="sel_type">
                <option value="name">名称</option>
                <option value="addr">地址</option>
                <option value="side">建设方</option>
            </select>
            <input id="txt_search" type="text" placeholder="回车搜索..." />
        </div>
        <div class="legend_wrap">
            <a class="close"></a>
            <div class="legend clearfix">
                <span class="floatL tag">车队</span>
                <ul class="legend_child">
                    <li class="img" id="legend_constructionWaste">
                       </li>
                    <li>渣土</li>
                </ul>
                <ul class="legend_child">
                    <li class="img" id="legend_mud">
                       </li>
                    <li>泥浆</li>
                </ul>
                 <ul class="legend_child">
                    <li class="img" id="legend_decoration">
                       </li>
                    <li>装修垃圾</li>
                </ul>
            </div>
            <div class="legend clearfix">
                <span class="floatL tag">工地</span>
                <ul class="legend_child">
                    <li class="img">
                        <img src="../Modules/images/进出土.png" /></li>
                    <li>进出土</li>
                </ul>
                <ul class="legend_child">
                    <li class="img">
                        <img src="../Modules/images/打桩.png" /></li>
                    <li>打桩</li>
                </ul>
                <ul class="legend_child">
                    <li class="img">
                        <img src="../Modules/images/开工.png" /></li>
                    <li>开工</li>
                </ul>
                <ul class="legend_child">
                    <li class="img">
                        <img src="../Modules/images/停工.png" /></li>
                    <li>停工</li>
                </ul>
                <%--<ul class="legend_child">
                    <li class="img">
                        <img src="../Modules/images/未验.png" /></li>
                    <li>未验</li>
                </ul>--%>
            </div>
              <div class="legend clearfix">
                <span class="floatL tag">渣土卸点</span>
                <ul class="legend_child">
                    <li class="img" id="legend_landfill">
                       </li>
                    <li>渣土</li>
                </ul>
               
            </div>
        </div>
        <div id="div_map"></div>
    </div>
</body>

</html>
<script>
    var RootPath = '<%=Request.ApplicationPath%>';
</script>
<script src="../js/seajs/sea.js"></script>
<script src="../js/seajs/seajs-preload.js"></script>
<script src="../js/seajs/seajs-css.js"></script>
<script src="../js/seajs-config.js"></script>
<script src="../Scripts/jquery-1.12.0.min.js"></script>
<script src="../Scripts/bootstrap.min.js"></script>
<script src="../js/common.js"></script>
<script src="../Modules/map/maptalks.js"></script>
<script src="../Modules/map/maptalks.heatlayer.js"></script>
<script src="../Modules/map/maptalks.clusterlayer.js"></script>
<script src="../Modules/map/maptalks.halolayer.js"></script>
<script src="../Modules/map/longjin.maptalks.js"></script>
<script src="../Modules/map/legend.js"></script>
<script>
    seajs.use("Modules/map/map.js", function () {
        alert("已过滤无法转换坐标");
    });
    seajs.use("Modules/map/map.menu.js", function (e) {
        
    });
</script>
