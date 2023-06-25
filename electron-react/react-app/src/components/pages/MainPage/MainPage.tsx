import { useEffect, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { IOrderDateSearch } from "./types";
import { useFormik } from "formik";
import { IOrderSearchResult } from "../Order/types";
import http from "../../../http_common";
import classNames from "classnames";

const MainPage = () => {
    const [searchParams, setSearchParams] = useSearchParams();
    const [profit, setProfit] = useState<number>(0);
    const projectId = localStorage.chosenProjectId;

    const [search, setSearch] = useState<IOrderDateSearch>({
        day: searchParams.get("day") || "",
        month: searchParams.get("month") || "",
        year: searchParams.get("year") || "",
        projectId: searchParams.get("projectId") || Number(projectId),
        page: searchParams.get("page") || 1,
    });

    const onSubmitSearch = (values: IOrderDateSearch) => {
        console.log("Проводимо пошук", values);
        setSearch(values);
    }

    const formik = useFormik({
        initialValues: search,
        onSubmit: onSubmitSearch
    });

    const { values, touched, errors, handleSubmit, handleChange } = formik;

    const [searchResult, setSearchResult] = useState<IOrderSearchResult>({
        orders: [],
        pages: 0,
        currentPage: 0,
        total: 0,
    });

    useEffect(() => {
        http.get<IOrderSearchResult>(`api/order/search-by-date`, {
            params: search
        })
            .then(resp => {
                const { data } = resp;
                console.log("Server response", data);
                setSearchResult(data);
            }).catch(err => {
                console.log("ERR", err);
            })
    }, [search])

    useEffect(() => {
        setProfit(0);
        searchResult.orders.forEach(element => {
            setProfit(prev => prev + element.totalPrice);
        });
    }, [searchResult])

    const { orders, pages, currentPage } = searchResult;

    const buttons = [];
    for (let i = 1; i <= pages; i++) {
        buttons.push(i);
    }

    const pagination = buttons.map(x => {
        return (
            <li className={classNames(
                "page-item",
                { "active": x === currentPage }
            )} key={x}>
                <Link className="page-link" to={`?page=${x}`} onClick={() => setSearch({ ...search, page: x })}>
                    {x}
                </Link>
            </li>
        );
    })

    const tableViewList = orders.map((item) => {
        return (
            <tr key={item.id}>
                <th scope="row">{item.id}</th>
                <td>{item.customer.firstName}&nbsp;{item.customer.secondName}</td>
                <td>{item.customer.email} <b>|</b> {item.customer.phone}</td>
                <td>{item.product.name}</td>
                <td>{item.product.manufacturerName}</td>
                <td>{item.productQuantity}</td>
                <td>{item.totalPrice}</td>
                <td>{item.sellDate.toLocaleString()}</td>
            </tr>
        )
    })


    return (
        <>
            <h1 className="text-center">Головна сторінка</h1>

            <form onSubmit={handleSubmit} className="mb-5">
                <div className="row">
                    <div className="col-md-2">
                        <label htmlFor="day" className="form-label">День</label>
                        <input
                            type="number"
                            id="day"
                            name="day"
                            value={values.day}
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.day && touched.day }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="month" className="form-label">Місяць</label>
                        <input
                            type="text"
                            id="month"
                            name="month"
                            value={values.month}
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.month && touched.month }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2">
                        <label htmlFor="year" className="form-label">Рік</label>
                        <input
                            type="number"
                            id="year"
                            name="year"
                            value={values.year}
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.year && touched.year }
                            )}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="col-md-2" style={{ display: 'flex', alignItems: 'end' }}>
                        <button type="submit" className="btn btn-primary">Пошук</button>
                    </div>
                </div>
            </form>

            <h3 className="text-success">PROFIT: {profit}</h3>
            <table className="table text-center">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Користувач</th>
                        <th scope="col">Пошта | Телефон</th>
                        <th scope="col">Продукт</th>
                        <th scope="col">Виробник</th>
                        <th scope="col">Кількість</th>
                        <th scope="col">Сумма ₴</th>
                        <th scope="col">Дата продажу</th>
                    </tr>
                </thead>
                <tbody>
                    {tableViewList}
                </tbody>
            </table>

            <nav aria-label="..." style={{ display: 'flex', justifyContent: 'end' }}>
                <ul className="pagination">
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == 1 }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) - 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) - 1 })} className="fa fa-arrow-left text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                    {pagination}
                    <li className={classNames(
                        "page-item",
                        { "disabled": search.page == buttons.length }
                    )}>
                        <span className="page-link">
                            <Link to={`?page=${Number(search.page) + 1}`} onClick={() => setSearch({ ...search, page: Number(search.page) + 1 })} className="fa fa-arrow-right text-primary" style={{ textDecoration: "none" }} aria-hidden="true"></Link>
                        </span>
                    </li>
                </ul>
            </nav>
        </>
    )
}

export default MainPage;