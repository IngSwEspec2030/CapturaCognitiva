import 'package:shared_preferences/shared_preferences.dart';

class Prefs {

    static final Prefs _instancia =
      new Prefs._internal();

  factory Prefs() {
    return _instancia;
  }

  Prefs._internal();

  SharedPreferences _prefs;

  init() async {
    _prefs = await SharedPreferences.getInstance();
  }

  bool get logged => _prefs.getBool("logged") ?? false;

  set logged(bool value) => _prefs.setBool("logged", value);

  String get usetToken => _prefs.getString("usetToken") ?? false;

  set usetToken(String value) => _prefs.setString("usetToken", value);

}