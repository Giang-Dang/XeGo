import 'dart:developer';

import 'package:sqflite/sqlite_api.dart';
import 'package:sqflite/sqflite.dart' as sql;
import 'package:path/path.dart' as path;

class StoredScheduledRideServices {
  static const kScheduledRideTbName = 'scheduled_ride';
  static Database? _db;

  Future<dynamic> getAllScheduledRideBydriverId(String driverId) async {
    try {
      _db ??= await _getDatabase();
      final queryRes = await _db!.query(kScheduledRideTbName,
          where: 'driverId = ?', whereArgs: [driverId]);

      log('getAllScheduledRideBydriverId: Done!');
      return queryRes;
    } catch (e) {
      log('getAllScheduledRideBydriverId error:');
      log(e.toString());
      return [];
    }
  }

  Future<bool> insert(String driverId, int rideId, String pickupTime) async {
    try {
      _db ??= await _getDatabase();
      _db!.insert(kScheduledRideTbName, {
        'driverId': driverId,
        'rideId': rideId,
        'pickupTime': pickupTime,
      });
      log("StoredScheduledRideServices > insert: Done!");

      return true;
    } catch (e) {
      log("StoredScheduledRideServices > insert error:");
      log(e.toString());
      return false;
    }
  }

  Future<bool> deleteRide(String driverId, int rideId) async {
    try {
      _db ??= await _getDatabase();

      int rowCount = await _db!.delete(
        kScheduledRideTbName,
        where: 'driverId = ? and rideId = ?',
        whereArgs: [driverId, rideId],
      );
      log('StoredScheduledRideServices > deleteRide affects $rowCount row(s).');
      return true;
    } catch (e) {
      log('StoredScheduledRideServices > deleteRide error: ${e.toString()}.');
      return false;
    }
  }

  Future<bool> deleteUser(String driverId, int rideId) async {
    try {
      _db ??= await _getDatabase();

      int rowCount = await _db!.delete(
        kScheduledRideTbName,
        where: 'driverId = ?',
        whereArgs: [driverId],
      );
      log('StoredScheduledRideServices > deleteUser affects $rowCount row(s).');
      return true;
    } catch (e) {
      log('StoredScheduledRideServices > deleteUser error: ${e.toString()}.');
      return false;
    }
  }

  Future<Database> _getDatabase() async {
    final dbPath = await sql.getDatabasesPath();
    final db = await sql.openDatabase(
      path.join(dbPath, '$kScheduledRideTbName.db'),
      onCreate: (db, version) {
        return db.execute(
            'CREATE TABLE $kScheduledRideTbName(driverId TEXT, rideId INTEGER, pickupTime TEXT, PRIMARY KEY(driverId, rideId))');
      },
      version: 1,
    );
    return db;
  }
}
