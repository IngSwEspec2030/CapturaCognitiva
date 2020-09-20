import 'package:get/get.dart';

class RxMyModel {
  final id = 0.obs;
  final nombres = ''.obs;
  final userId = ''.obs;
  final response = ''.obs;
  final message = ''.obs;
  final errors = ''.obs;
}

class UsuarioModel {
  UsuarioModel({id, nombres, userId, response, message, errors});

  final rx = RxMyModel();

  get id => rx.id.value;
  set id(value) => rx.id.value = value;

  get nombres => rx.nombres.value;
  set nombres(value) => rx.nombres.value = value;

  get userId => rx.nombres.value;
  set userId(value) => rx.nombres.value = value;

  get response => rx.nombres.value;
  set response(value) => rx.nombres.value = value;

  get message => rx.nombres.value;
  set message(value) => rx.nombres.value = value;

  get errors => rx.nombres.value;
  set errors(value) => rx.nombres.value = value;

  factory UsuarioModel.fromJson(Map<String, dynamic> json) => UsuarioModel(
        userId: json["userId"],
        nombres: json["nombres"],
        id: json["id"],
        response: json["response"],
        message: json["message"],
        errors: json["errors"],
      );

  Map<String, dynamic> toJson() => {
        "userId": userId,
        "nombres": nombres,
        "id": id,
        "response": response,
        "message": message,
        "errors": errors,
      };
}
