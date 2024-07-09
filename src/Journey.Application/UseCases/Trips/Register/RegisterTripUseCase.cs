﻿using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Exception;
using Journey.Exception.ExceptionsBase;
using Journey.Infrastructure;
using Journey.Infrastructure.Entities;

namespace Journey.Application.UseCases.Trips.Register
{
    public class RegisterTripUseCase
    {

        public ResponseShortTripJson Execute(RequestRegisterTripJson request)
        {
            Validate(request);
            var dbContecxt = new JourneyDbContext();

            var entity = new Trip
            {
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,

            };

            dbContecxt.Trips.Add(entity);
            dbContecxt.SaveChanges();

            return new ResponseShortTripJson
            {
                EndDate = entity.EndDate,
                StartDate = entity.StartDate,
                Name = entity.Name,
                Id = entity.Id
            };

        }

        private void Validate(RequestRegisterTripJson request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new JourneyException(ResourceErrorMessages.NAME_EMPTY);
            }

            //validacao com hora sempre usar utc pois pega a hora base do planeta
            if (request.StartDate.Date < DateTime.UtcNow.Date)
            {
                throw new JourneyException(ResourceErrorMessages.DATE__TRIP_MUST_BE_LATER_THAN_TODAY);
            }

            if (request.EndDate.Date < request.StartDate.Date)
            {
                throw new JourneyException(ResourceErrorMessages.END_DATE_TRIP_MUST_BE_LATER_START_DATE);
            }


        }

    }
}