# Utility
#### 介绍
* DotNet开发企业级Helper类库.封装了常用的功能性代码,且类与类之间没有依赖性,每个类都可以单独拿来使用.
* 使用本套类库可以增加代码的优雅性和阅读性,让coder编码可以更舒心,更加适用于有强迫的人（ps:作者是一个有强迫癌症的人...）
* 本套类库尚不完善，作者会陆陆续续更新其他内容
#### 使用该类库的好处
1. 封装变化点,在企业中,我们经常会应对大量的变化,我们将变化点封装起来,一旦需求改变,便于维护和修改
2. 快捷方式,操作起来比较方便快捷,增加代码优雅性
3. 集中做容错,这点相信我不需要过多解释
#### 为什么说使用该类库可以增加代码的优雅性和阅读性？
作者就不王婆卖瓜，自卖自夸先，我们直接看例子

* 类型转换

		//普通将string类型转换成其他类型
			string str = "123";
			int num =  Convert.ToInt32(str);
			//这样就将string转成了int类型,但是如果str里面包含非数字(例如:"123ab")的话,那么会报异常
		
		//使用本类库优化版
			string str = "123";
			int num = str.ToInt();
			//如果转换失败这回返回int的默认值0,当然,你也可以设置默认值
			int num = str.ToInt(-1);

* 正则表达式
		
		//普通使用正则表达式提取数字
			string str = "OH!!,HelloWordl,My Phone is 13060517782";
            Match match = Regex.Match(str, @"[0-9]+");
            if (match.Success)
            {
               string result = match.Value;
            }

		//使用本类库
			string result = RegexHelper.Match(str,@"[0-9]+");

* SQL操作
	
		//普通将SqlDataReader的内容读取到一个List<Person>中
			List<Person> list = new List<Person>();
			while(reader.Read())
			{
				Person p = new Person();
				p.Id =reader.GetInt(reader.GetOrdinal("id"));
				p.name =reader.GetString(reader.GetOrdinal("name"));
				p.birthDate =reader.GetDateTime(reader.GetOrdinal("birthDate"));
				list.Add(p);
			}
		
		//使用本类库 将SqlDataReader的内容读取到一个List<Person>中
			List<Person> list = SqlHelper.MapEntity<Person>(reader);

##### 下面是类库的内容,有对应类文件的已经可以使用,其余的会陆陆续续更新

1. cookie操作 --------- CookieHelper.cs
2. session操作 ------- SessionHelper.cs
3. cache操作
4. ftp操作
5. http操作 ------------ HttpHelper.cs
6. json操作 ------------ JsonHelper.cs		
7. xml操作 ------------- XmlHelper.cs
8. Excel操作			
9. Sql操作 ------------- SqlHelper.cs
10. 类型转换 ------------ Converter.cs
11. 加密解密 ------------ EncryptHelper.cs	
12. 邮件发送	------------ MailHelper.cs
13. 二维码
14. 汉字转拼音
15. 计划任务	------------ IntervalTask.cs
16. 信息配置 ------------ Setting.cs
17. 上传下载
18. 视频转换
19. 图片操作
20. 验证码生成
21. String拓展 ---------- StringExtension.cs
22. 正则表达式 --------- RegexHelper.cs
23. 分页操作
24. UBB编码
25. Url重写
26. Object拓展 --------- ObjectExtension.cs
27. Stream的拓展	------ StreamExtension.cs

##### 最后
如果发现Bug或者有不合理的地方或者建议,请发送邮箱至：228494894@qq.com

