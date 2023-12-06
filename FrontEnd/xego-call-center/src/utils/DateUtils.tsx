import { utcToZonedTime, zonedTimeToUtc } from "date-fns-tz";

export function convertUtcToVn(date: string | Date): Date {
  const parsedDate = typeof date === "string" ? new Date(date + 'Z') : date;
  return utcToZonedTime(parsedDate, "Asia/Ho_Chi_Minh");
}

export function convertVnToUtc(date: string | Date): Date {
  const parsedDate = typeof date === "string" ? new Date(date) : date;
  return zonedTimeToUtc(parsedDate, "Asia/Ho_Chi_Minh");
}
