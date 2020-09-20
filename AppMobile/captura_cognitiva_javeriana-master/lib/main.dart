import 'package:capturacognitivajaveriana/app/data/repositories/local_storage.dart';
import 'package:get/get.dart';
import 'package:flutter/material.dart';
import 'package:capturacognitivajaveriana/app/routes/app_pages.dart';
 
void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  final _prefs = Prefs();
  await _prefs.init();
  runApp(MyApp());
}
 
class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      title: 'Captura Cognitiva App',
      theme: ThemeData(
        primaryColor: Colors.indigo[900],
        accentColor: Colors.amber
      ),
      initialRoute: Routes.SPLASH,
      getPages: AppPages.pages,
    );
  }
}