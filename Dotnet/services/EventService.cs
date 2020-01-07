using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Event;
using Sabio.Models.Requests.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class EventService : IEventService
    {
        IDataProvider _data = null;

        public EventService(IDataProvider data)
        {
            _data = data;
        }
        public int Add(EventAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[Events_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    col.Add(idOut);

                    AddCommonParams(model, col);
                }, returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;

                    int.TryParse(oId.ToString(), out id);

                });
            return id;
        }
        public Event GetById(int Id)
        {
            string procName = "[dbo].[Events_SelectById]";

            Event events = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", Id);
            }, delegate (IDataReader reader, short set)
            {
                events = MapEvents(reader);
            });
            return events;
        }
        public Paged<Event> SelectAll (int pageIndex, int pageSize)
        {
            Paged<Event> pagedResult = null;
            List<Event> result = null;
            Event aEvent = null;

            int totalCount = 0;
            _data.ExecuteCmd(
                "dbo.Events_SelectAll",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@pageIndex", pageIndex);
                    parameterCollection.AddWithValue("@pageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    aEvent = MapEvents(reader);

                    if(totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(14);
                    }
                    if (result == null)
                    {
                        result = new List<Event>();
                    }
                    result.Add(aEvent);
                });
            if (result != null)
            {
                pagedResult = new Paged<Event>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }
        public void Update(EventUpdateRequest model)
        {
            string procName = "[dbo].[Events_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);
                    AddCommonParams(model, col);
                }, returnParameters: null);
        }
        private static Event MapEvents(IDataReader reader)
        {
            Event aEvents = new Event();
            int startingIndex = 0;
            aEvents.Id = reader.GetSafeInt32(startingIndex++);
            aEvents.EventTypeId = reader.GetSafeInt32(startingIndex++);
            aEvents.Name = reader.GetSafeString(startingIndex++);
            aEvents.Summary = reader.GetSafeString(startingIndex++);
            aEvents.ShortDescription = reader.GetSafeString(startingIndex++);
            aEvents.VenueId = reader.GetSafeInt32(startingIndex++);
            aEvents.EventStatusId = reader.GetSafeInt32(startingIndex++);
            aEvents.ImageUrl = reader.GetSafeString(startingIndex++);
            aEvents.ExternalSiteUrl = reader.GetSafeString(startingIndex++);
            aEvents.IsFree = reader.GetSafeBool(startingIndex++);
            aEvents.DateCreated = reader.GetSafeUtcDateTime(startingIndex++);
            aEvents.DateModified = reader.GetSafeUtcDateTime(startingIndex++);
            aEvents.DateStart = reader.GetSafeUtcDateTime(startingIndex++);
            aEvents.DateEnd = reader.GetSafeUtcDateTime(startingIndex++);
            return aEvents;

        }
        private static void AddCommonParams(EventAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@EventTypeId", model.EventTypeId);
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Summary", model.Summary);
            col.AddWithValue("@ShortDescription", model.ShortDescription);
            col.AddWithValue("@VenueId", model.VenueId);
            col.AddWithValue("@EventStatusId", model.EventStatusId);
            col.AddWithValue("@ImageUrl", model.ImageUrl);
            col.AddWithValue("@ExternalSiteUrl", model.ExternalSiteUrl);
            col.AddWithValue("@IsFree", model.IsFree);
            col.AddWithValue("@DateStart", model.DateStart);
            col.AddWithValue("@DateEnd", model.DateEnd);
        }

    }
}
