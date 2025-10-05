namespace StudentServicesWebApi.Domain.Enums;

/// <summary>
/// Types of actions that can be performed by admins
/// </summary>
public enum AdminActionType
{
    /// <summary>
    /// User role was changed (User to Admin or Admin to User)
    /// </summary>
    RoleChange = 0,
    
    /// <summary>
    /// Balance was added to user account
    /// </summary>
    BalanceAdd = 1,
    
    /// <summary>
    /// Balance was subtracted from user account
    /// </summary>
    BalanceSubtract = 2,
    
    /// <summary>
    /// User account was suspended/unsuspended
    /// </summary>
    AccountStatusChange = 3,
    
    /// <summary>
    /// Other admin action
    /// </summary>
    Other = 999
}