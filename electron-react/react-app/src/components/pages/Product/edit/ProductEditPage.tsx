import { Link, useNavigate, useParams } from "react-router-dom";
import { IProductEditItem, IProductItem } from "../types";
import { useFormik } from "formik";
import { ProductEditSchema } from "../validation";
import { useEffect, useState } from "react";
import http from "../../../../http_common";
import classNames from "classnames";
import { IManufacturerItem, IManufacturerServiceResponse } from "../../Manufacturer/types";

const ProductEditPage = () => {
    const navigate = useNavigate();

    const { productId, manufacturerId } = useParams();

    const [manufacturers, setManufacturers] = useState<IManufacturerItem[]>([]);
    const [product, setProduct] = useState<IProductItem>({
        id: 0,
        name: "",
        manufacturerId: 0,
        manufacturerName: ""
    })
    const init: IProductEditItem = {
        id: Number(productId),
        name: "",
        manufacturerId: 0
    };

    const onEditSubmit = (values: IProductEditItem) => {
        console.log("VALUES", values);
        http.put("api/product/update", values)
            .then(resp => {
                console.log("RESP", resp)
                navigate("..");
            })
            .catch(err => {
                console.log("ERR", err);
                const { data } = err.response;
                setFieldError("name", data);
            })
    }

    const formik = useFormik({
        initialValues: init, // данні які передаєм (початкові налаштування для полів)
        onSubmit: onEditSubmit, // метод, який спрацює при submit форми і коли усі дані у форміку валідні
        validationSchema: ProductEditSchema // схема валідації даних
    })

    const { errors, values, touched, setFieldValue, setFieldError, handleChange, handleSubmit } = formik;

    useEffect(() => {

        http.get<IManufacturerServiceResponse>("api/manufacturer/index")
            .then(resp => {
                const { payload } = resp.data;
                console.log("MANUFACTURER LIST", resp);
                const manufs = payload as IManufacturerItem[]
                setManufacturers(manufs);
                http.get(`api/product/${productId}`)
                    .then(resp1 => {
                        const { payload } = resp1.data;
                        console.log("GET PRODUCT BY ID", payload);
                        setProduct(payload);
                        setFieldValue("name", payload.name);
                        setFieldValue("manufacturerId", payload.manufacturerId);
                    })
                    .catch(err => {
                        console.log("ERR", err);
                    })
            })
            .catch(err => {
                console.log("ERR MANLST", err);
            });
    }, []);


    return (
        <>
            <h1 className="text-center">Редагування продукту</h1>

            <form onSubmit={handleSubmit} className="col-md-6 offset-md-3">
                <div className="mb-3">
                    <label htmlFor="name" className="form-label">Назва</label>
                    <input type="text" id="name" name="name" className={classNames(
                        "form-control",
                        { "is-invalid": errors.name && touched.name }
                    )} value={values.name} onChange={handleChange} placeholder="Введіть ім'я" />
                    {errors.name && touched.name && (
                        <div className="invalid-feedback">
                            {errors.name}
                        </div>
                    )}
                </div>

                <div className="mb-3">
                    <label htmlFor="manufacturerId" className='form-label'>Оберіть Виробника</label>
                    <select
                        className={classNames(
                            "form-select",
                            { "is-invalid": errors.manufacturerId && touched.manufacturerId }
                        )}
                        aria-label="Manufacturer select"
                        defaultValue={values.manufacturerId} // Значення, яке міститься в select
                        value={values.manufacturerId}
                        onChange={handleChange} // Якщо значення міняється, воно записується у формік
                        name="manufacturerId" // Значення поля у форміку = manufacturerId - якщо його не буде - formik - не буде нічого добавлять
                        id='manufacturerId' // Це використовується для label
                    >
                        {/* Значення, яке завжди буде не обране, для того, щоб нагадать, що треба вказувать категорію */}
                        <option value="0" disabled>---</option> {/* disabled - щоб при виборі категорії ти не міг обрати цей варік */}
                        {/* Перебираємо список категорій і виводимо їх у вигляді options */}
                        {manufacturers.map(item => (
                            <option key={item.id} value={item.id}>{item.name}</option>
                        ))}
                    </select>
                    {(errors.manufacturerId && touched.manufacturerId) && (
                        <span className='text-danger'>{errors.manufacturerId}</span>
                    )}
                </div>

                <div className="container d-flex flex-row justify-content-center gap-3">
                    <button type="submit" className="btn btn-primary">Редагувати</button>
                    <Link to="../" className="btn btn-warning">Відхілити</Link>
                </div>
            </form>
        </>
    )
}

export default ProductEditPage;