import 'package:get/get.dart';
// Pages
import 'package:capturacognitivajaveriana/app/modules/splash/splash_page.dart';
import 'package:capturacognitivajaveriana/app/modules/login/login_page.dart';
import 'package:capturacognitivajaveriana/app/modules/reset_password/reset_page.dart';
import 'package:capturacognitivajaveriana/app/modules/home/home_page.dart';
import 'package:capturacognitivajaveriana/app/modules/capture/captura_page.dart';
import 'package:capturacognitivajaveriana/app/modules/result/result_page.dart';

part './app_routes.dart';

abstract class AppPages {
  static final pages = [
    GetPage(
      name: Routes.SPLASH,
      page: () => SplashPage(),
    ),
    GetPage(
      name: Routes.LOGIN,
      page: () => LoginPage(),
    ),
    GetPage(
      name: Routes.RESET,
      page: () => ResetPage(),
    ),
    GetPage(
      name: Routes.HOME,
      page: () => HomePage(),
    ),
    GetPage(
      name: Routes.CAPTURA,
      page: () => CapturaPage(),
    ),
    GetPage(
      name: Routes.RESULT,
      page: () => ResultPage(),
    ),
  ];
}
