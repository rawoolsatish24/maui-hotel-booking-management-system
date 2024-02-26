using HotelBookingManagementSystem.Models;
using Microsoft.Maui.ApplicationModel.Communication;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HotelBookingManagementSystem.Repository
{
    public class HBMSService : IHBMSRepository
    {
        public async Task<int> CreateUser(UserMaster objUserMaster)
        {
            try
            {
                HttpClient objHttpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7142/api/UserMaster")
                };
                objHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                objHttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                StringContent apiContent = new StringContent(JsonConvert.SerializeObject(objUserMaster), Encoding.UTF8, "application/json");
                HttpResponseMessage apiResponse = await objHttpClient.PostAsync("", apiContent);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return int.Parse(apiResponse.Content.ReadAsStringAsync().Result);
                }
                else { return -1; }
            } catch { return -108; }
        }

        public async Task<Object> ValidateLogin(String email, String password)
        {
            try
            {
                HttpClient objHttpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7142/api/UserMaster/ValidateLogin")
                };
                string apiUrl = $"?email={email}&password={password}";
                HttpResponseMessage apiResponse = await objHttpClient.GetAsync(apiUrl);
                if (apiResponse.IsSuccessStatusCode)
                {
                    string responseContent = apiResponse.Content.ReadAsStringAsync().Result;
                    UserMaster loginUserMaster = JsonConvert.DeserializeObject<UserMaster>(responseContent);
                    return await Task.FromResult(loginUserMaster);
                } else { return -1; }
            } catch { return -108; }
        }

        public async Task<Object> SearchUser(String userId)
        {
            try
            {
                HttpClient objHttpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7142/api/UserMaster/SearchUser")
                };
                string apiUrl = $"?userId={userId}";
                HttpResponseMessage apiResponse = await objHttpClient.GetAsync(apiUrl);
                if (apiResponse.IsSuccessStatusCode)
                {
                    string responseContent = apiResponse.Content.ReadAsStringAsync().Result;
                    UserMaster loginUserMaster = JsonConvert.DeserializeObject<UserMaster>(responseContent);
                    return await Task.FromResult(loginUserMaster);
                }
                else { return -1; }
            }
            catch { return -108; }
        }

        public async Task<Object> UpdateUser(UserMaster objUserMaster)
        {
            try
            {
                HttpClient objHttpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7142/api/UserMaster")
                };
                objHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                objHttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                StringContent apiContent = new StringContent(JsonConvert.SerializeObject(objUserMaster), Encoding.UTF8, "application/json");
                HttpResponseMessage apiResponse = await objHttpClient.PutAsync("", apiContent);

                if (apiResponse.IsSuccessStatusCode)
                {
                    string responseContent = apiResponse.Content.ReadAsStringAsync().Result;
                    UserMaster updatedUserMaster = JsonConvert.DeserializeObject<UserMaster>(responseContent);
                    return updatedUserMaster;
                } else { return -1; }
            } catch { return -108; }
        }

        public async Task<int> DeleteUser(String userId)
        {
            try
            {
                HttpClient objHttpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7142/api/UserMaster")
                };
                string apiUrl = $"?UserId={userId}";
                HttpResponseMessage apiResponse = await objHttpClient.DeleteAsync(apiUrl);
                if (apiResponse.IsSuccessStatusCode)
                {
                    return await DeleteBookings(userId);
                }
                else { return -1; }
            }
            catch { return -108; }
        }

        public async Task<int> AddBooking(BookingHistory objBookingHistory)
        {
            try
            {
                HttpClient objHttpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7142/api/BookingHistory")
                };
                objHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                objHttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                StringContent apiContent = new StringContent(JsonConvert.SerializeObject(objBookingHistory), Encoding.UTF8, "application/json");
                HttpResponseMessage apiResponse = await objHttpClient.PostAsync("", apiContent);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return int.Parse(apiResponse.Content.ReadAsStringAsync().Result);
                }
                else { return -1; }
            }
            catch { return -108; }
        }

        public async Task<Object> GetUserBookingHistory(String userId)
        {
            try
            {
                HttpClient objHttpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7142/api/BookingHistory")
                };
                string apiUrl = $"?userId={userId}";
                HttpResponseMessage apiResponse = await objHttpClient.GetAsync(apiUrl);
                if (apiResponse.IsSuccessStatusCode)
                {
                    string responseContent = apiResponse.Content.ReadAsStringAsync().Result;
                    List<BookingHistory> userBookingHistory = JsonConvert.DeserializeObject<List<BookingHistory>>(responseContent);
                    return userBookingHistory;
                }
                else { return -1; }
            }
            catch { return -108; }
        }

        public async Task<int> UpdateBooking(BookingHistory objBookingHistory)
        {
            try
            {
                HttpClient objHttpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7142/api/BookingHistory")
                };
                objHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                objHttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                StringContent apiContent = new StringContent(JsonConvert.SerializeObject(objBookingHistory), Encoding.UTF8, "application/json");
                HttpResponseMessage apiResponse = await objHttpClient.PutAsync("", apiContent);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return int.Parse(apiResponse.Content.ReadAsStringAsync().Result);
                }
                else { return -1; }
            }
            catch { return -108; }
        }

        public async Task<int> DeleteBookings(String userId)
        {
            try
            {
                HttpClient objHttpClient = new()
                {
                    BaseAddress = new Uri("https://localhost:7142/api/BookingHistory")
                };
                string apiUrl = $"?userId={userId}";
                HttpResponseMessage apiResponse = await objHttpClient.DeleteAsync(apiUrl);
                if (apiResponse.IsSuccessStatusCode)
                {
                    return int.Parse(apiResponse.Content.ReadAsStringAsync().Result);
                }
                else { return -1; }
            }
            catch { return -108; }
        }
    }
}
