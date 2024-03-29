﻿using Microsoft.AspNetCore.Mvc;
using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services.ServiceA
{
    public interface IAppointmentCurrentService
    {
        public Task<IEnumerable<AppointmentCurrent>?> UserAsync(int id);
        public Task<IEnumerable<AppointmentCurrent>?> SpecialistAsync(int id);
        public Task<AppointmentCurrent?> PostAppointmentAsync(AppointmentCurrent appointment);

        public Task<bool> DeleteAppointmentCurrentUser(int id);
    }
}
