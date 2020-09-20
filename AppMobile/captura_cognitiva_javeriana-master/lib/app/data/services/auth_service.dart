import 'dart:convert';
import 'package:capturacognitivajaveriana/app/data/services/database.dart';

class AuthService extends Database {
  Future<Map<String, dynamic>> login(String email, String password) async {
    final Uri uri = Uri.https(baseUrl, "api/Account/Login");

    var response = await httpClient.post(
      uri,
      headers: headers,
      body: jsonEncode({"Email": email, "Contraseña": password}),
    );
    final Map<String, dynamic> data = json.decode(response.body);
    return data;
  }

  Future<Map<String, dynamic>> resetPassword(String email) async {
    final Uri uri = Uri.https(baseUrl, "api/Account/RecoveryPassword");
    var response = await httpClient.post(
      uri,
      headers: headers,
      body: jsonEncode({"Email": email}),
    );
    final data = json.decode(response.body);
    return data;
  }
}
