using SpazaLink.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpazaLink.Shared.DTOs
{
    // REQUEST DTOs (What API receives)
    public class CreateTraderRequest
    {
        // Required for registration
        public string BusinessName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Location
        public string Area { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string? Landmark { get; set; }

        // Business details
        public TraderType Type { get; set; }
        public List<string> ProductCategories { get; set; } = new();

        // Contact Methods for notification
        public string? WhatsAppNumber { get; set; }
        public string? Email { get; set; }
    }

    public class UpdateTraderRequest
    {
        public string? BusinessName { get; set; }
        public string? OwnerName { get; set; }
        public List<string>? ProductCategories { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? Email { get; set; }
    }

    public class VerifyTraderRequest
    {
        public string IDNumber { get; set; } = string.Empty;
    }
    // RESPONSE DTOs (What API returns)
    public class TraderResponse
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public TraderType Type { get; set; }
        public List<string> ProductCategories { get; set; } = new();
        public TraderStatus Status { get; set; }
        public TraderTier Tier { get; set; }
        public bool IsVerified { get; set; }
        public DateTime RegisteredDate { get; set; }
    }

    public class TraderSummaryResponse  // For listing/search results
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public TraderType Type { get; set; }
        public TraderTier Tier { get; set; }
        public bool IsVerified { get; set; }
    }

    // Generic API Response
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
            => new() { Success = true, Data = data, Message = message };

        public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
            => new() { Success = false, Message = message, Errors = errors };
    }
}
