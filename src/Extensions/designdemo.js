//yfb smalljscode 1.0.0.0 http://www.yinyue200.com/zh-cn/appList/yfxsrt/yfbfiles.aspx
//packagename:示例支持包
//authorname:yinyue200.com
//copyright:© 2016 yinyue200.com
//usingnamespace:HtmlAgilityPackRT
//supporturl:^http://www.cmxsw.com/
//searchname:示例点1$示例点2
//feedbackurl:mailto:yfreader@yinyue200.com
//minreaderversion:1.3.0.0
//end
//packagename必须是唯一的
//可以多次声明searchname,使用$分隔，这样阅读器就会多次调用getSiteSearchUri。也可以不声明，这样表示不支持搜索，只能使用网站解析功能。packagename需要在全软件内唯一。packagename应该可以作为文件夹的名称使用.usingnamespace代表引用的命名空间，多个命名空间用空格“ ”分隔。supporturl是支持网站的正则表达式，若不符合该表达式阅读器将不会调用该插件。也可以置空。但为了性能考虑，不要置空;minreaderversion表示最低阅读器版本
var WebSiteEngine = {
    createNew: function(nowurl,starturl){
        var temp = {};
        temp.siteName = "cmxsw.com"
        temp.getChapterContent = function (html) {
            var hd = new HtmlAgilityPackRT.HtmlDocument();
            hd.loadHtml(html);
 
            return HtmlAgilityPackRT.YFHelper.contentDecoding( hd.documentNode.selectSingleNode("//div[@id='content']").innerText)//根据参数html返回章节内容
            //可能会抛出的错误
            throw {type:"",innerException:obj/*可选*/,displaymessage:obj/*可选，该属性会向用户显示,且如果提供displaymessage属性，将替换默认的错误提示*/,context:""/*可选，重定向时可能要记录的信息，将在新页面中附带该信息在getChapterContentWithContext的context参数中*/,helpLink:""/*可选，重定向地址，与type:"RESET"配合重定向*/}
            //可用的type值
            // 该章节是图片章节，目前不支持解析
            //"GIF",
            //解析失败
            //"JIEXI"
            // 需要支付，但当前不支持
            //"PAY"
            // 需要转到新的网址获取数据
            //"RESET"
        }
        temp.getChapterList=function(html){
            //根据参数html返回章节列表，以\r\n分隔
            //例如
            var hd = new HtmlAgilityPackRT.HtmlDocument();
            hd.loadHtml(html);
            return HtmlAgilityPackRT.YFHelper.helpForeachNodes(hd.documentNode.selectNodes("//div[@id='xsbody']//a"),nowurl)
        }
        temp.getBookInfo=function(html){
            //根据参数html返回书籍简介
            var hd = new HtmlAgilityPackRT.HtmlDocument();
            hd.loadHtml(html);
            var title = hd.documentNode.selectSingleNode("//title").innerText;
            return { name: title.substring(0, title.indexOf("最新章节")), infotext: hd.documentNode.selectSingleNode("//div[@id='AdsT1']").innerText, bookImagePath: "", authorName: "书籍作者名称" }
        }
        temp.getSiteSearchUri=function(searchcontent,searchpoint){
            var dic = new Array({ key:"a",value:"b"});
            return {name:"http://www.qq.com",httphead:dic};
 
            //switch(searchpoint){
            //    case "示例点1":
            //    case "示例点2":
            //        return {name:"示例点2的搜索链接地址"};
            //}      
        }
        temp.getBookListFormSiteSearch=function(htmlcontent){
            return new Array({ name: '小说1名字', path: "链接", info: "概览" }, { name: '小说2名字', path: "链接", info: "概览" },{name:'小说3名字',path:"链接",info:"概览"});//示例
            //这里给出的小说地址在用户点击项目后将通过isChapterListPage函数，若该函数返回true，将直接进入书籍简介页引发getBookInfo函数，若返回false，将访问地址获取可能的重定向(来自重定向请求或者getNewUriToGo函数)来重新isChapterListPage
        }
        temp.isChapterListPage=function(){//可选，默认返回true
            return true;             
        }
        temp.getNewUriToGo=function(htmlcontent){//可选，返回NULL即不需要重定向
            return null; //重定向的新URL
        }
        temp.getChapterContentWithContext=function(htmlcontent,context){//可选
            return "小说内容";
        }
        temp.isSupport=function(){//可选，指示当前URL是否是支持的URL，若不支持，阅读器可能会使用其它解析程序来解析内容
            return true;
        }
        //未完成，待添加
        return temp;
    }
};