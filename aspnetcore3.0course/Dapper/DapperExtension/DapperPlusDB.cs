using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DapperExtension
{
    /// <summary>
    /// IDapperPlusDB数据库类型 
    /// </summary>
    public class DapperPlusDB : IDapperPlusDB
    {
        /// <summary>
        /// 连接对象
        /// </summary>
        IDbConnection _dbConnection;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="dbConnection">连接对象</param>
        /// <param name="connectionString">连接字符串</param>
        public DapperPlusDB(IDbConnection dbConnection, DataBaseType dataBaseType = DataBaseType.None)
        {
            DataBaseType = dataBaseType;
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType { get; }



        /// <summary>
        /// 连接对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return _dbConnection;
        }
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <typeparam name="T">映射实体类</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered">是否缓存结果</param>
        /// <param name="commandTimeout">command超时时间(秒)</param>
        /// <param name="commandType">command类型</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = false, int? commandTimeout = null, CommandType? commandType = null)
        {
            IEnumerable<T> result = null;
            PollyInvock(() =>
            {
                result = _dbConnection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            });
            return result;
        }
        /// <summary>
        /// 查询异步方法
        /// </summary>
        /// <typeparam name="T">映射实体类</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="transaction">事务</param> 
        /// <param name="commandTimeout">command超时时间(秒)</param>
        /// <param name="commandType">command类型</param>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            IEnumerable<T> result = null;
            await PollyInvockAsync(async () =>
            {
                result = await _dbConnection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
            });
            return result;
        }


        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="sql">映射实体类</param>
        /// <param name="param">参数对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">command超时时间(秒)</param>
        /// <param name="commandType">command类型</param>
        /// <returns></returns>
        public int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var result = 0;
            PollyInvock(() =>
            {
                result = _dbConnection.Execute(sql, param, transaction, commandTimeout, commandType);
            });
            return result;
        }

        /// <summary>
        /// 异步执行方法
        /// </summary>
        /// <param name="sql">映射实体类</param>
        /// <param name="param">参数对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">command超时时间(秒)</param>
        /// <param name="commandType">command类型</param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var result = 0;
            await PollyInvockAsync(async () =>
            {
                result = await _dbConnection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
            });
            return result;
        }


        /// <summary>
        /// 查询单值
        /// </summary>
        /// <typeparam name="T">映射实体类</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">command超时时间(秒)</param>
        /// <param name="commandType">command类型</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var result = default(T);
            PollyInvock(() =>
            {
                result = _dbConnection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
            });
            return result;
        }

        /// <summary>
        /// 异步查询单值
        /// </summary>
        /// <typeparam name="T">映射实体类</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">command超时时间(秒)</param>
        /// <param name="commandType">command类型</param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var result = default(T);
            await PollyInvockAsync(async () =>
            {
                result = await _dbConnection.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
            });
            return result;
        }


        /// <summary>
        /// 重试调用
        /// </summary>
        /// <param name="action"></param>
        private void PollyInvock(Action action)
        {
            var policy = Policy
                .Handle<Exception>()
                .Retry(3, (excetpion, index, context) =>
                {
                });
            policy.Execute(() =>
            {
                action();
            });
        }
        /// <summary>
        /// 异常重试调用
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        private async Task PollyInvockAsync(Func<Task> func)
        {
            var policy = Policy
                .Handle<Exception>()
                .RetryAsync(3, (excetpion, index, context) =>
                { });
            await policy.ExecuteAsync(() =>
            {
                return func();
            });
            //return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Dispose();
            }
        }
    }
}
