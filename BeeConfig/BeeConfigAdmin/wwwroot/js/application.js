
/*
 * 初始化整体页面
 * 1 初始化菜单
 * 
 * */
$(function () { 

    //初始菜单页面页面
    var navigation=[
        {
            isActive:true,
            name:"首页",
            url:"#",
            icon:"fa-dashboard",
        },
        {
            isActive:false,
            name:"系统管理",
            url:"",
            icon:"fa-dashboard",
            subs:[
                {
                    isActive:false,
                    name:"用户管理",
                    icon:"fa-th",
                    url:"beeservice/bee_user.html",
                },
                {
                    isActive:false,
                    name:"角色管理",
                    icon:"fa-th",
                    url:"sys.html",
                },
                {
                    isActive:false,
                    name:"权限管理",
                    icon:"fa-th",
                    url:"sys.html",
                },
            ]
        },
        {
            isActive:false,
            name:"业务管理",
            icon:"fa-pie-chart",
            url:"",
            subs:[
                {
                    isActive:false,
                    name:"环境管理",
                    icon:"fa-th",
                    url:"beeservice/bee_env.html",
                },
                {
                    isActive:false,
                    name:"应用管理",
                    icon:"fa-th",
                    url:"beeservice/bee_app.html",
                },
                {
                    isActive:false,
                    name:"配置管理",
                    icon:"fa-th",
                    url:"beeservice/bee_config.html",
                },
                {
                    isActive: false,
                    name: "日志管理",
                    icon: "fa-th",
                    url: "beeservice/bee_log.html",
                }
            ]
        },
        {
            isActive:false,
            name:"系统设置",
            icon:"fa-th",
            url:"#",
            subs:[]
        }
    ];
    var navigateHtml=[];
    getNaviHtml(navigation,navigateHtml);    
    $("ul.sidebar-menu").html(navigateHtml.join(''));
 
    //注册菜单点击事件 
    $("li").on("click",".menuli",function(){
        $("li.menuli").each(function(){ $(this).removeClass("active"); });
        $(this).addClass("active");
    });

    $('#content_wrapper').multitabs({
        iframe:true,
        init:[{                                       
            type :'info',                           //content type, may be main | info, if empty, default is 'info'
            title : '',                         //title of tab, if empty, show the URL
            content: '',                        //content html, the url value is useless if set the content.
            url : ''                            //URL
        },]
    });
})
 
/* 
 * 根据json对象生成菜单栏html代码
 * 
 * */
function getNaviHtml(list,navigateHtml){

    for(var i=0;i<list.length;i++){
        var temp=list[i];
        if(temp.subs && temp.subs.length>0){
            var activeClass="";
            var hasSubs="menuli";
            if(temp.isActive){
                activeClass="active menu-open ";
            }             
            if(temp.subs && temp.subs.length>0){
                hasSubs="";
            } 

            navigateHtml.push('<li class=" '+activeClass+hasSubs+' treeview">');

            navigateHtml.push('<a href="#" >');
            navigateHtml.push('<i class="fa '+temp.icon+'"></i> <span>'+temp.name+'</span>');
            navigateHtml.push('<span class="pull-right-container">');
            navigateHtml.push('<i class="fa fa-angle-left pull-right"></i>');
            navigateHtml.push('</span>');
            navigateHtml.push('</a>');
            navigateHtml.push('<ul class="treeview-menu">');             
            getNaviHtml(temp.subs,navigateHtml);            
            navigateHtml.push('</ul>');
            navigateHtml.push('</li>');    
        }
        else{
            if(temp.isActive){
                navigateHtml.push('<li class="active menuli treeview">');
            }
                
            else{
                navigateHtml.push('<li class="menuli treeview">');
            } 
            navigateHtml.push('<a href="'+temp.url+'" class="multitabs">');
            navigateHtml.push('<i class="fa '+temp.icon+'"></i> <span>'+temp.name+'</span>');
            navigateHtml.push('<span class="pull-right-container">');
            navigateHtml.push('<small class="label pull-right bg-green"></small>');
            navigateHtml.push('</span>');
            navigateHtml.push('</a>');
            navigateHtml.push('</li>');
        }
    }
}