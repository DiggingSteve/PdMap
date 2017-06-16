var drawTools = (function () {
    function DrawTool() {

    }
   
        
    DrawTool.prototype.drawCircle = function (obj) {
        /// <summary>绘制圆形</summary>     
        /// <param name="obj" type="String">对象包含id=>容器id,canvasWidth=>画布宽度,canvasHeight=>画布高度,x,y=>分别距离左上角距离,radius=>半径,color=>颜色</param>        
        var dom = document.getElementById(obj.id);
        if (!!!dom) return false;
        var canvas = document.createElement("canvas");
        dom.appendChild(canvas);
        canvas.width = obj.canvasWidth;
        canvas.height = obj.canvasHeight;
        var context = canvas.getContext('2d');
        //  context.fillStyle = "#EEEEFF";
        // context.fillRect(0, 0, 400, 300);
        context.beginPath();
        context.arc(obj.x, obj.y, obj.radius, 0, Math.PI * 2, true);// 第一个参数距离画布左上角 x轴距离,第二个参数距离画布左上角y轴距离，半径，开始的弧度，结束弧度，false顺时针
        context.closePath();
        context.fillStyle = obj.color;
        context.fill();

    };

    DrawTool.prototype.drawDiamond = function (obj) {
        var dom = document.getElementById(obj.id);
        if (!!!dom) return false;
        var canvas = document.createElement("canvas");
        dom.appendChild(canvas);
        canvas.width = obj.canvasWidth;
        canvas.height = obj.canvasHeight;
        var context = canvas.getContext('2d');
        context.translate(obj.canvasWidth / 2, obj.canvasHeight / 2);
        context.rotate(Math.PI / 4);
        context.translate(-obj.canvasWidth/2,-obj.canvasHeight/2)
        
        context.fillRect((obj.canvasWidth- obj.w)/2,(obj.canvasHeight- obj.h)/2, obj.w, obj.h);
     
        context.fillStyle = obj.color;
        context.save();
    }

    return DrawTool;
})();
