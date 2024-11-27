using System.Net.Mail;
using EternalBAND.Api.Services;
using EternalBAND.DataAccess;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EternalBAND.Api;

 public interface IBaseEmailSender : IEmailSender
 {
 }