using DevTech.Shared.Domain.Model.Errors;

namespace DevTech.CustomerManagement.Domain.Model.Errors;

public static class CustomerManagementErrors
{
    public static readonly Error CustomerCreationFailed =
        Error.Failure(
            "CustomerManagement.CustomerCreationFailed",
            "An error occurred while creating the customer.");

    public static readonly Error LaundryCreationFailed =
        Error.Failure(
            "CustomerManagement.LaundryCreationFailed",
            "An error occurred while creating the laundry.");

    public static readonly Error CustomerNotFound =
        Error.NotFound(
            "CustomerManagement.CustomerNotFound",
            "The specified customer was not found.");

    public static readonly Error LaundryNotFound =
        Error.NotFound(
            "CustomerManagement.LaundryNotFound",
            "The specified laundry was not found.");

    public static readonly Error DuplicateCustomer =
        Error.Conflict(
            "CustomerManagement.DuplicateCustomer",
            "A customer already exists with the specified identity.");

    public static readonly Error InvalidCustomerId =
        Error.Validation(
            "CustomerManagement.InvalidCustomerId",
            "Customer id must be non-empty.");

    public static readonly Error InvalidLaundryId =
        Error.Validation(
            "CustomerManagement.InvalidLaundryId",
            "Laundry id must be non-empty.");

    public static readonly Error InvalidCustomerName =
        Error.Validation(
            "CustomerManagement.InvalidCustomerName",
            "Customer name is not valid.");

    public static readonly Error InvalidLaundryName =
        Error.Validation(
            "CustomerManagement.InvalidLaundryName",
            "Laundry name is required.");

    public static readonly Error InvalidPhoneNumber =
        Error.Validation(
            "CustomerManagement.InvalidPhoneNumber",
            "Phone number is not valid.");

    public static readonly Error InvalidDocumentType =
        Error.Validation(
            "CustomerManagement.InvalidDocumentType",
            "Document type is not valid.");

    public static readonly Error InvalidAddress =
        Error.Validation(
            "CustomerManagement.InvalidAddress",
            "Address is not valid.");

    public static Error CustomerNotFoundWithId(Guid customerId) =>
        Error.NotFound(
            "CustomerManagement.CustomerNotFound",
            $"The customer with id '{customerId}' was not found.");

    public static Error LaundryNotFoundWithId(Guid laundryId) =>
        Error.NotFound(
            "CustomerManagement.LaundryNotFound",
            $"The laundry with id '{laundryId}' was not found.");
}