import { Link, useNavigate } from "react-router-dom";
import { ICustomerCreateItem } from "../types";
import { useFormik } from "formik";
import { CustomerCreateSchema } from "../validation";
import classNames from "classnames";
import http from "../../../../http_common";
import { useState } from "react";

const CustomerCreatePage = () => {
    const navigate = useNavigate();

    const projectId = window.localStorage.chosenProjectId;
    const init: ICustomerCreateItem = {
        firstName: "",
        secondName: "",
        email: "",
        phone: "",
        address: ""
    };

    const onCreateSubmit = async (values: ICustomerCreateItem) => {
        console.log("CREATE VALUES", values);
        http.post("api/customer/create", values)
            .then(resp => {
                console.log("RESP", resp);
                const { payload } = resp.data;
                http.post(`api/CustomerProject/create/customer-${Number(payload.id)}/project-${projectId}`)
                    .then(resp => {
                        console.log("RESP", resp)
                        navigate("..");
                    }).catch(err => {
                        console.log("ERR", err);
                    })
            })
            .catch(err => {
                const { data } = err.response;
                console.log("ERR", data);
                if (data.message.includes("Пошта")) setFieldError("email", data.message);
                else setFieldError("phone", data.message);
            })
    }

    const formik = useFormik({
        initialValues: init,
        onSubmit: onCreateSubmit,
        validationSchema: CustomerCreateSchema
    })

    const { errors, values, touched, handleChange, handleSubmit, setFieldError } = formik;

    return (
        <>
            <h1 className="text-center">Стоворення Користувача</h1>

            <form onSubmit={handleSubmit} className="col-md-6 offset-md-3">
                <div className="mb-3 row">
                    <div className="col-md-6">
                        <label htmlFor="firstName" className="form-label">Ім'я <i className="fa fa-snowflake-o text-danger fa-2xs" aria-hidden="true"></i></label>
                        <input type="text" id="firstName" name="firstName" className={classNames(
                            "form-control",
                            { "is-invalid": errors.firstName && touched.firstName }
                        )} value={values.firstName} onChange={handleChange} placeholder="Введіть ім'я" />
                        {errors.firstName && touched.firstName && (
                            <div className="invalid-feedback">
                                {errors.firstName}
                            </div>
                        )}
                    </div>
                    <div className="col-md-6">
                        <label htmlFor="secondName" className="form-label">Фімілія <i className="fa fa-snowflake-o text-danger fa-2xs" aria-hidden="true"></i></label>
                        <input type="text" id="secondName" name="secondName" className={classNames(
                            "form-control",
                            { "is-invalid": errors.secondName && touched.secondName }
                        )} value={values.secondName} onChange={handleChange} placeholder="Введіть Фамілію" />
                        {errors.secondName && touched.secondName && (
                            <div className="invalid-feedback">
                                {errors.secondName}
                            </div>
                        )}
                    </div>
                </div>
                <div className="mb-3">
                    <label htmlFor="email" className="form-label">Пошта <i className="fa fa-snowflake-o text-danger fa-2xs" aria-hidden="true"></i></label>
                    <input id="email" name="email" className={classNames(
                        "form-control",
                        { "is-invalid": errors.email && touched.email }
                    )} value={values.email} onChange={handleChange} placeholder="Введіть пошту" />
                    {errors.email && touched.email && (
                        <div className="invalid-feedback">
                            {errors.email}
                        </div>
                    )}
                </div>
                <div className="mb-3">
                    <label htmlFor="phone" className="form-label">Телефон <i className="fa fa-snowflake-o text-danger fa-2xs" aria-hidden="true"></i></label>
                    <input id="phone" name="phone" className={classNames(
                        "form-control",
                        { "is-invalid": errors.phone && touched.phone }
                    )} value={values.phone} onChange={handleChange} placeholder="Введіть телефон" />
                    {errors.phone && touched.phone && (
                        <div className="invalid-feedback">
                            {errors.phone}
                        </div>
                    )}
                </div>
                <div className="mb-3">
                    <label htmlFor="address" className="form-label">Адреса</label>
                    <input id="address" name="address" className={classNames(
                        "form-control",
                        { "is-invalid": errors.address && touched.address }
                    )} value={values.address} onChange={handleChange} placeholder="Введіть адреса" />
                    {errors.address && touched.address && (
                        <div className="invalid-feedback">
                            {errors.address}
                        </div>
                    )}
                </div>
                <div className="container d-flex flex-row justify-content-center gap-3">
                    <button type="submit" className="btn btn-primary">Створити</button>
                    <Link to="../" className="btn btn-warning">Відхілити</Link>
                </div>
            </form>
        </>
    )
}

export default CustomerCreatePage;