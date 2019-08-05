﻿using DonationBoard.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationBoard.Services
{
    public class MockSessionService : ISessionService
    {
        private SQLiteAsyncConnection conn;
        private readonly ISQLiteService _sQLiteService;

        public MockSessionService(ISQLiteService sQLiteService)
        {
            _sQLiteService = sQLiteService;

            conn = _sQLiteService.GetConnection();

            conn.CreateTableAsync<Session>().Wait();
        }

        public async Task<Guid> InitializeSession(Guid savedSessionId)
        {
            // if saved session is found in db, leave method
            var storedSession = await GetSession(savedSessionId);
            if (storedSession != null)
                return savedSessionId;

            // try to find the latest session
            var latestSession = await conn.Table<Session>().OrderByDescending(s => s.CreateDate).FirstOrDefaultAsync();
            if (latestSession != null)
            {
                return latestSession.Id;
            }

            // if no latest session exists, create a new one
            var newSessionId = await CreateSession();
            return newSessionId;
        }

        public async Task<Guid> CreateSession()
        {
            Guid newSessionId = Guid.NewGuid();

            await conn.InsertAsync(new Session { Id = newSessionId, CreateDate = DateTime.Now });

            return newSessionId;
        }

        public async Task<Session> GetSession(Guid sessionId)
        {
            return await conn.Table<Session>().FirstOrDefaultAsync(s => s.Id == sessionId);
        }

        public async Task<ObservableCollection<Session>> GetSessions()
        {
            var sessions = await conn.Table<Session>().OrderByDescending(s => s.CreateDate).ToListAsync();

            return new ObservableCollection<Session>(sessions);
        }
    }
}
