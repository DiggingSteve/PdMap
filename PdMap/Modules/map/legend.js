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
        ///<summary>绘制一个以size为边长的正方形画布内部嵌入一个正方形旋转45度，并且四个顶点在画布各边的中点</summary>
     
        /// <param name='id' type='String'>容器id</param>
        /// <param name='size' type='Number'> 正方形大小</param>
        /// <param name='color' type='String' >颜色</param>
   
        var dom = document.getElementById(obj.id);
        var size = obj.size;
        var squareSize = Math.sqrt(Math.pow(size / 2, 2)*2); //内部正方形的边长
        if (!!!dom) return false;
        var canvas = document.createElement("canvas");
        dom.appendChild(canvas);
        canvas.width = size;
        canvas.height = size;
        var context = canvas.getContext('2d');
        context.translate(size / 2, size / 2);//移动画布中心到正方形中心点
        context.rotate(Math.PI / 4);//旋转45度
        context.translate(-squareSize / 2, -squareSize / 2) //中心点移动到画布边的中点
        context.fillStyle = obj.color;
        context.fillRect(0,0,squareSize,squareSize);//
        
        context.save();
    }

    return DrawTool;
})();
