# 如何在越飞阅读中运行SiteD插件

## 1、对 site.xml 文件做必要的修改

1.将require中的item标签中的 url 属性删去

2.添加yfpath属性，属性内容为原url属性的的JavaScript文件名

## 2、打包（可选）

如果你的 SiteD 的 require 标签不是空的，则需要进行打包

1.打开smallpackage打包器，将档案名改为你的插件名称（应与你的SiteD插件的title标签相同）

2.将你的 site.xml 插件和所有 require 中引用的依赖放在同一空文件夹下，依赖的名称与之前修改的yfpath属性相同

3.重命名 site.xml 文件为 ddcat.xml

4.点击打包器的选择文件夹按钮，选择你刚才处理的文件夹

## 3、运行

如果已经打包，可双击导入阅读器

如果未经打包，更改文件扩展名为yfb即可导入阅读器

## 需要注意的内容

1.目前越飞阅读会暂时忽略ua,guid,encode,reward,login等标签

2.待补充
