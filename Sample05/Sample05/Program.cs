using Dapper;
using Sample05.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Sample05
{
    class Program
    {
        private static readonly string connstring = @"Data Source=127.0.0.1;User ID=sa;Password=123456;Initial Catalog=Czar.Cms;Pooling=true;Max Pool Size=100;";
        static void Main(string[] args)
        {
            test_insert();
            test_mult_insert();

            test_del();
            test_mult_del();

            test_update();
            test_mult_update();

            test_select_one();
            test_select_list();

            test_select_content_with_comment();

            Console.Read();
        }

        static void test_select_content_with_comment()
        {
            using (var conn = new SqlConnection(connstring))
            {
                string sql_insert = @"select * from content where id=@id;select * from comment where content_id=@id;";
                using (var result = conn.QueryMultiple(sql_insert, new { id = 5 }))
                {
                    var content = result.ReadFirstOrDefault<Content>();
                    content.comments = result.Read<Comment>();
                    Console.WriteLine($"test_select_content_with_comment:内容5的评论数量{content.comments.Count()}");
                }

            }
        }

        /// <summary>
        /// 查询单条指定的数据
        /// </summary>
        static void test_select_one()
        {
            using (var conn = new SqlConnection(connstring))
            {
                string sql_insert = @"select * from [dbo].[content] where id=@id";
                var result = conn.QueryFirstOrDefault<Content>(sql_insert, new { id = 5 });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }

        /// <summary>
        /// 查询多条指定的数据
        /// </summary>
        static void test_select_list()
        {
            using (var conn = new SqlConnection(connstring))
            {
                string sql_insert = @"select * from [dbo].[content] where id in @ids";
                var result = conn.Query<Content>(sql_insert, new { ids = new int[] { 6, 7 } });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }


        /// <summary>
        /// 测试修改单条数据
        /// </summary>
        static void test_update()
        {
            var content = new Content
            {
                id=5,
                title = "标题55",
                content = "内容55"
            };
            using (var conn = new SqlConnection(connstring))
            {
                var sql = @"update [content] set title=@title,[content]=@content,modify_time=getdate() where (id=@id)";
                var result = conn.Execute(sql, content);
                Console.WriteLine($"test_update：修改了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试批量修改单条数据
        /// </summary>
        static void test_mult_update()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    id = 6,
                    title = "标题66",
                    content = "内容66"
                },
                new Content
                {
                    id = 7,
                    title = "标题77",
                    content = "内容77"
                }
            };
            
            using (var conn = new SqlConnection(connstring))
            {
                var sql = @"update [content] set title=@title,[content]=@content,modify_time=getdate() where (id=@id)";
                var result = conn.Execute(sql, contents);
                Console.WriteLine($"test_mult_update：修改了{result}条数据！");
            }
        }


        /// <summary>
        /// 测试单条数据插入
        /// </summary>

        static void test_insert()
        {
            var content = new Content
            {
                title = "标题1",
                content = "内容1"
            };
            using (var conn = new SqlConnection(connstring))
            {
                var sql_insert = @"INSERT INTO [content] ([title],[content],[status],[add_time],[modify_time]) VALUES(@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert:插入了{result}条数据！");
            }
        }
        /// <summary>
        /// 一次批量插入多条数据
        /// </summary>
        static void test_mult_insert()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    title = "批量插入标题1",
                content = "批量插入内容1"
                },
                 new Content
                {
                    title = "批量插入标题2",
                content = "批量插入内容2"
                }
            };

            using (var conn = new SqlConnection(connstring))
            {
                var sql_insert = @"INSERT INTO [content] ([title],[content],[status],[add_time],[modify_time]) VALUES(@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_insert:插入了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试删除单条数据
        /// </summary>
        static void test_del()
        {
            var content = new Content
            {
                id = 2
            };

            using (var conn = new SqlConnection(connstring))
            {
                var sql = $"DELETE FROM [Content] WHERE (id=@id)";
                var result = conn.Execute(sql, content);
                Console.WriteLine($"test_del：删除了{result}条数据！");

            }
        }
        /// <summary>
        /// 测试批量删除多条数据
        /// </summary>
        static void test_mult_del()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    id = 3
                },
                new Content
                {
                    id = 4
                }
            };

            using (var conn = new SqlConnection(connstring))
            {
                var sql = $"DELETE FROM [Content] WHERE (id=@id)";
                var result = conn.Execute(sql, contents);
                Console.WriteLine($"test_mult_del：删除了{result}条数据！");

            }
        }
    }
}
