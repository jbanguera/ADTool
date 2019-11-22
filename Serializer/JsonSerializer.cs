using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Newtonsoft.Json.Converters;

namespace ADTool.Serializer
{
    /// <summary>
    /// Clase para la serialización/deserialización en JSON
    /// </summary>
    public class JsonSerializer
    {
        #region - C O N S T R U C T O R
        /// <summary>
        /// Constructor
        /// </summary>
        private JsonSerializer() { }
        #endregion

        #region - D E S E R I A L I Z E  
        /// <summary>
        /// Deserialize to <see cref="JObject"/>.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <exception cref="JsonReaderException"></exception>
        public static JObject DeserializeToJObj(string json)
        {
            try
            {
                JObject jObject = JObject.Parse(json);
                return jObject;
            }catch(JsonReaderException ex) { throw ex; }
        }

        /// <summary>
        /// Deserialize to dynamic object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static dynamic DeserializeToExpandoObject(string json)
        {
            try
            {
                var expConverter = new ExpandoObjectConverter();
                dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(json, expConverter);
                return obj;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Deserialize to dynamic object.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static dynamic DeserializeToDynamic(string json)
        {
            try
            {
                dynamic dynObj = JsonConvert.DeserializeObject(json);
                return dynObj;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Deserialize to T object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T Deserialize<T>(string json)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Deserialize to especific object.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="typeObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object Deserialize(string json, Type typeObj)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject(json, typeObj);
                return obj;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Convert <see cref="DataSet "/> on especific object. 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="typeObj"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object ConvertDataSetToObj(DataSet ds, Type typeObj)
        {
            try
            {
                var json = Serialize(ds);
                var objT = Deserialize(json, typeObj);
                return objT;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Convert <see cref="DataSet "/> on especific object. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T ConvertDataSetToObj<T>(DataSet ds)
        {
            try
            {
                var json = Serialize(ds);
                var objT = Deserialize<T>(json);
                return objT;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Convierte un DataSet en una entidad de tipo "T1" con una lista de entidades hijas de tipo "T2"
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T1 GetEntity<T1, T2>(DataSet ds)
        {
            try
            {
                var ents = (T1)Activator.CreateInstance(typeof(T1));
                ds.DataSetName = typeof(T1).Name;
                ds.Tables[0].TableName = $"{typeof(T2).Name}List";

                var obj = ConvertDataSetToObj(ds, typeof(T1));
                if (obj != null) ents = (T1)obj;

                return ents;
            }
            catch (Exception ex) { throw ex; }
        }

        public static T MapToNew<T>(object dataObject) where T : new()
        {
            return Deserialize<T>(Serialize(dataObject));
        }

        /// <summary>
        /// Get Entity
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="ds"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T1 GetEntity<T1, T2>(DataSet ds, Type type)
        {
            try
            {
                var ents = (T1)Activator.CreateInstance(type);
                ds.DataSetName = typeof(T1).Name;
                ds.Tables[0].TableName = $"{typeof(T2).Name}List";

                var obj = ConvertDataSetToObj(ds, typeof(T1));
                if (obj != null) ents = (T1)obj;

                return ents;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Dado un DataTable, devuelve una lista de entidades de tipo "T"
        /// </summary>
        /// <typeparam name="T">El tipo de la lista de entidades</typeparam>
        /// <param name="dt">El DataTable de datos de entrada</param>
        /// <returns></returns>
        public static List<T> GetEntity<T>(DataTable dt)
        {
            var ents = new List<T>();
            var json = Serialize(dt);
            var obj = Deserialize(json, typeof(List<T>));
            if (obj != null) ents = (List<T>)obj;
            return ents;
        }

        /// <summary>
        /// Get Entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<T> GetEntity<T>(DataSet ds)
        {
            var ents = new List<T>();
            if (ds.Tables.Count <= 0) return ents;
            var dt = ds.Tables[0];
            var json = Serialize(dt);
            var obj = Deserialize(json, typeof(List<T>));
            if (obj != null) ents = (List<T>)obj;
            return ents;
        }

        /// <summary>
        /// Dado un DataTable devuelve una entidad de tipo T, que corresponde al primer registro de la tabla
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T GetFirstEntity<T>(DataTable dt) where T : new()
        {
            var json = Serialize(dt);
            var obj = (List<T>)Deserialize(json, typeof(List<T>));
            if (obj == null) return new T();
            var ents = obj;
            return ents.First();
        }


        #endregion

        #region - S E R I A L I Z E 
        /// <summary>
        /// Serializa un objeto
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }
        #endregion     

    }
}