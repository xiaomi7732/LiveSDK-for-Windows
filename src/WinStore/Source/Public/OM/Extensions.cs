// ------------------------------------------------------------------------------
// Copyright (c) 2014 Microsoft Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
// ------------------------------------------------------------------------------

namespace Microsoft.Live.OM
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Extensions
    {
        /// <summary>
        /// Get contacts out of LiveOperationResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IEnumerable<Contact> GetContacts(this LiveOperationResult result)
        {
            try
            {
                dynamic dynamicResult = result.Result;
                List<object> data = ((List<object>)dynamicResult.data).ToList();
                List<Contact> convertedList = new List<Contact>();
                foreach (dynamic contact in data)
                {
                    Contact newContact = new Contact();
                    newContact.Id = contact.id ?? null;
                    newContact.FirstName = contact.first_name ?? null;
                    newContact.LastName = contact.last_name ?? null;
                    newContact.Name = contact.name ?? null;
                    newContact.IsFriend = contact.is_friend ?? false;
                    newContact.IsFavorite = contact.is_favorite ?? false;
                    newContact.UserId = contact.user_id ?? null;
                    newContact.UpdatedTime = DateTimeOffset.Parse(contact.updated_time);

                    if (contact.email_hashes != null)
                    {
                        newContact.EmailHashes = new List<string>();
                        foreach (dynamic hash in contact.email_hashes)
                        {
                            newContact.EmailHashes.Add((string)hash);
                        }
                    }

                    if (contact.emails != null)
                    {
                        newContact.Emails = new Dictionary<string, string>();
                        foreach (KeyValuePair<string, object> email in contact.emails)
                        {
                            if (email.Value != null)
                            {
                                newContact.Emails.Add(email.Key, email.Value.ToString());
                            }
                        }
                    }

                    convertedList.Add(newContact);
                }

                return convertedList;
            }
            catch (Exception)
            {
                // For debug use only.
                throw;
            }
        }
    }
}
