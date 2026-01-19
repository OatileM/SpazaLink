using System;
using System.Collections.Generic;
using System.Text;

namespace SpazaLink.Shared.Exceptions
{
    public class SpazaLinkException : Exception
    {
        public string ErrorCode { get; }
        public SpazaLinkException(string message, string errorCode = "GENERIC_ERROR"):
            base(message)
        {
            ErrorCode = errorCode;
        }
    }

    public class TraderNotFoundException : SpazaLinkException
    {
        public TraderNotFoundException(Guid traderId) :
            base($"Trader with ID {traderId} was not found.", "TRADER_NOT_FOUND")
        {
        }
        public TraderNotFoundException(string phoneNumber) :
            base($"Trader with phone number {phoneNumber} was not found.", "TRADER_NOT_FOUND")
        {
        }
    }

    public class TraderAlreadyExistsException : SpazaLinkException
    {
        public TraderAlreadyExistsException(string phoneNumber):
            base($"Trader with phone number {phoneNumber} already exists.", "TRADER_ALREADY_EXISTS")
        {
        }
    }

    public class TraderVerificationException : SpazaLinkException
    {
        public TraderVerificationException(string message) :
            base(message, "TRADER_VERIFICATION_FAILED")
        {
        }
    }

    public class InvalidTraderDataException : SpazaLinkException
    {
        public InvalidTraderDataException(string fieldName, string reason) :
            base($"Invalid {fieldName}: {reason}", "INVALID_TRADER_DATA")
        {
        }
    }
}
