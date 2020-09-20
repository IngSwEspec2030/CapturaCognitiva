import 'package:http/http.dart' as http;

abstract class Database {
  String _baseUrl;
  http.Client _httpClient;
  Map<String, String> _headers;

  set headers(Map value) => _headers = value;

  Database() {
    this._baseUrl = 'capturacognitiva.azurewebsites.net';
    this._httpClient = http.Client();
    this._headers = {
      'Content-Type': 'application/json',
      'Token': 'akyumnsdfo89715472--//',
    };
  }

  String get baseUrl => this._baseUrl;

  http.Client get httpClient => this._httpClient;

  Map<String, String> get headers => this._headers;

}
